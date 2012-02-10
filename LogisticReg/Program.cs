using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace LogisticReg
{
    class Program
    {
        static void Main(string[] args)
        {
            Regression();
        }

        static void Test()
        {
            LogisticReg regression = new LogisticReg(2, 1, 1);
            List<DataItem> items = new List<DataItem>();
            StreamReader reader = new StreamReader(new FileStream("ex2data2.txt", FileMode.Open));
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] ss = line.Split(',');
                DataItem item = new DataItem();
                item.AddItem(0, double.Parse(ss[0]));
                item.AddItem(1, double.Parse(ss[1]));
                item.AddClassLabel(int.Parse(ss[2]));
                items.Add(item);
            }
            double[] theta = { 0, 0 };
            double r = regression.CostFunc(items, theta);
        }

        static void Regression()
        {
            //List<DataItem> training = LoadData(0, "training", 50);
            List<DataItem> training = LoadData(0, "crsvalid", 50);
            LogisticReg regression = new LogisticReg(50, 0, 1);
            regression.Training(training);
            List<DataItem> crsvalid = LoadData(0, "training", 50);
            double precision, recall;
            regression.Testing(training, out precision, out recall);
            regression.Testing(crsvalid, out precision, out recall);
            Console.WriteLine("precision: {0}, recall: {1}", precision, recall); 
        }

        static List<DataItem> LoadData(int classLabel, string type, int numTopics)
        {
            MongoServer server = MongoServer.Create();
            MongoDatabase db = server.GetDatabase("docmodel");
            MongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>("ldamodel_" + type + "_" + classLabel + "_" + numTopics);
            MongoCursor<BsonDocument> cursor = coll.FindAll();
            List<DataItem> items = new List<DataItem>();
            foreach (BsonDocument doc in cursor)
            {
                DataItem item = new DataItem();
                items.Add(item);
                item.DataItemId = doc["DocID"].AsString;                
                if (!doc["ClassLabels"].IsBsonNull)
                {
                    foreach (BsonValue cl in doc["ClassLabels"].AsBsonArray)
                    {
                        item.AddClassLabel(cl.AsInt32);                        
                    }
                }
                
                if (!doc["WordCounts"].IsBsonNull)
                {
                    foreach (BsonDocument kvp in doc["WordCounts"].AsBsonArray)
                    {                  
                        foreach (BsonElement e in kvp)
                        {
                            item.AddItem(e.Value.AsInt32, 1);                            
                        }
                    }
                }
                item.Normalize();                
            }
            cursor = null;
            coll = null;
            db = null;
            server.Disconnect();
            return items;
        }
    }


}

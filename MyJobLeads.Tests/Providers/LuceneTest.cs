using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace MyJobLeads.Tests.Providers
{
    [TestClass]
    public class LuceneTest
    {
        [TestMethod]
        public void Can_Get_Lucene_Working()
        {
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MyJobLeadsLuceneTests";
            var indexDirectory = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(directoryPath));

            // Build the index
            var writer = new IndexWriter(indexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), true, IndexWriter.MaxFieldLength.UNLIMITED);

            try
            {
                // Add 2 documents
                var doc1 = new Document();
                var doc2 = new Document();

                doc1.Add(new Field("id", "doc1", Field.Store.YES, Field.Index.ANALYZED));
                doc1.Add(new Field("content", "This is my first document", Field.Store.YES, Field.Index.ANALYZED));
                doc2.Add(new Field("id", "doc2", Field.Store.YES, Field.Index.ANALYZED));
                doc2.Add(new Field("content", "The big red", Field.Store.YES, Field.Index.ANALYZED));
                doc2.Add(new Field("content2", "fox jumped", Field.Store.YES, Field.Index.ANALYZED));

                writer.AddDocument(doc1);
                writer.AddDocument(doc2);

                writer.Optimize();
                writer.Close();

                // Search for doc2
                string queryString = "bag rad";
                queryString = queryString.Replace("~", string.Empty).Replace(" ", "~ ") + "~";

                var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, 
                                                        new string[] {"content", "content2"}, 
                                                        new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
                parser.SetDefaultOperator(QueryParser.Operator.AND);

                var query = parser.Parse(queryString);
                var searcher = new IndexSearcher(indexDirectory, true);
                var hits = searcher.Search(query);

                Assert.AreEqual(1, hits.Length());

                var document = hits.Doc(0);

                Assert.AreEqual("doc2", document.Get("id"));
                Assert.AreEqual("The big red", document.Get("content"));
            }
            finally
            {
                writer.Close();
            }
        }
    }
}

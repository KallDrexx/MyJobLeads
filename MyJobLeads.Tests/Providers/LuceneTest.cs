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
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MyJobLeadsLuceneTests";

            // Build the index
            var writer = new IndexWriter(directory, new StandardAnalyzer(), true);

            try
            {
                // Add 2 documents
                var doc1 = new Document();
                var doc2 = new Document();

                doc1.Add(new Field("id", "doc1", Field.Store.YES, Field.Index.ANALYZED));
                doc1.Add(new Field("content", "testing1", Field.Store.YES, Field.Index.ANALYZED));
                doc2.Add(new Field("id", "doc2", Field.Store.YES, Field.Index.ANALYZED));
                doc2.Add(new Field("content", "testing2", Field.Store.YES, Field.Index.ANALYZED));

                writer.AddDocument(doc1);
                writer.AddDocument(doc2);

                writer.Optimize();
                writer.Close();

                // Search for doc2
                var parser = new QueryParser("id", new StandardAnalyzer());
                var query = parser.Parse("doc2");
                var searcher = new IndexSearcher(directory);
                var hits = searcher.Search(query);

                Assert.AreEqual(1, hits.Length());

                var document = hits.Doc(0);

                Assert.AreEqual("doc2", document.Get("id"));
                Assert.AreEqual("testing2", document.Get("content"));
            }
            finally
            {
                writer.Close();
            }
        }
    }
}

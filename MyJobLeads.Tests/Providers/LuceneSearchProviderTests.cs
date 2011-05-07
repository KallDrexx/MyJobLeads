using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyJobLeads.DomainModel.Providers.Search;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using MyJobLeads.DomainModel.Entities;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Documents;

namespace MyJobLeads.Tests.Providers
{
    [TestClass]
    public class LuceneSearchProviderTests
    {
        private LuceneSearchProvider _provider;
        private FSDirectory _indexDirectory;

        private TopDocs SearchIndex(string searchField, string searchQuery, out IndexSearcher searcher)
        {
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, searchField, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            searcher = new IndexSearcher(_indexDirectory, true);
            return searcher.Search(parser.Parse(searchQuery), 2);
        }

        [TestInitialize]
        public void Setup()
        {
            _provider = new LuceneSearchProvider();

            // Load the index and delete all documents so it's fresh for the test
            _indexDirectory = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(_provider.LuceneIndexBaseDirectory));
            var writer = new IndexWriter(_indexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), true, IndexWriter.MaxFieldLength.UNLIMITED);
            writer.DeleteAll();
            writer.Close();
        }

        [TestMethod]
        public void Can_Index_Company()
        {
            // Setup
            Company company = new Company
            {
                Id = 3,
                City = "City",
                Industry = "Industry",
                MetroArea = "Metro",
                Name = "Name",
                Notes = "Notes",
                Phone = "Phone",
                State = "State",
                Zip = "Zip"
            };

            // Act
            _provider.Index(company);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.COMPANY_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.COMPANY_ID), "Document had an incorrect company id value");
            Assert.AreEqual("City", doc.Get(LuceneSearchProvider.Constants.COMPANY_CITY), "Document had an incorrect company city value");
            Assert.AreEqual("Industry", doc.Get(LuceneSearchProvider.Constants.COMPANY_INDUSTRY), "Document had an incorrect company industry value");
            Assert.AreEqual("Metro", doc.Get(LuceneSearchProvider.Constants.COMPANY_METRO), "Document had an incorrect metro area value ");
            Assert.AreEqual("Name", doc.Get(LuceneSearchProvider.Constants.COMPANY_NAME), "Document had an incorrect name value");
            Assert.AreEqual("Notes", doc.Get(LuceneSearchProvider.Constants.COMPANY_NOTES), "Document had an incorrect notes value");
            Assert.AreEqual("Phone", doc.Get(LuceneSearchProvider.Constants.COMPANY_PHONE), "Document had an incorrect phone value");
            Assert.AreEqual("State", doc.Get(LuceneSearchProvider.Constants.COMPANY_STATE), "Document had an incorrect state value");
            Assert.AreEqual("Zip", doc.Get(LuceneSearchProvider.Constants.COMPANY_ZIP), "Document had an incorrect zip value");
        }

        [TestMethod]
        public void Indexed_Companys_Null_Values_Become_Empty_Strings()
        {
            // Setup
            Company company = new Company { Id = 3 };

            // Act
            _provider.Index(company);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.COMPANY_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.COMPANY_ID), "Document had an incorrect company id value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_CITY), "Document had an incorrect company city value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_INDUSTRY), "Document had an incorrect company industry value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_METRO), "Document had an incorrect metro area value ");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_NAME), "Document had an incorrect name value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_NOTES), "Document had an incorrect notes value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_PHONE), "Document had an incorrect phone value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_STATE), "Document had an incorrect state value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.COMPANY_ZIP), "Document had an incorrect zip value");
        }

        [TestMethod]
        public void Can_Index_Multiple_Companies()
        {
            // Setup
            var company1 = new Company { Id = 3, Name = "Name" };
            var company2 = new Company { Id = 4, Name = "Name" };

            // Act
            _provider.Index(company1);
            _provider.Index(company2);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.COMPANY_NAME, "Name", out searcher);

            Assert.AreEqual(2, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        public void ReIndexing_Company_Updates_Existing_Company_Document()
        {
            // Setup
            Company company = new Company { Id = 3, Name = "Name1" };
            _provider.Index(company);
            company.Name = "Name2";

            // Act
            _provider.Index(company);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.COMPANY_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("Name2", doc.Get(LuceneSearchProvider.Constants.COMPANY_NAME), "Document had an incorrect name");
        }

        [TestMethod]
        public void Can_Remove_Company_From_Index()
        {
            // Setup
            Company company = new Company { Id = 3 };
            _provider.Index(company);

            // Act
            _provider.Remove(company);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.COMPANY_ID, "3", out searcher);

            Assert.AreEqual(0, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Indexing_Null_Company_Throws_ArgumentNullException()
        {
            // Act
            _provider.Index((Company)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Removing_Null_Company_Throws_ArgumentNullException()
        {
            // Act
            _provider.Remove((Company)null);
        }
    }
}
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
using MyJobLeads.DomainModel.ViewModels;

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
            bool createNewIndex = !IndexReader.IndexExists(_indexDirectory);
            var writer = new IndexWriter(_indexDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29), createNewIndex, IndexWriter.MaxFieldLength.UNLIMITED);
            writer.DeleteAll();
            writer.Close();
        }

        #region Indexing Tests

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
        public void Indexed_Companys_Null_Properties_Become_Empty_Strings()
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

        [TestMethod]
        public void Can_Index_Contact()
        {
            // Setup
            Contact contact = new Contact
            {
                Id = 3,
                Assistant = "Assistant",
                DirectPhone = "Direct Phone",
                Email = "Email",
                Extension = "Ext",
                MobilePhone = "MobilePhone",
                Name = "Name",
                Notes = "Notes",
                ReferredBy = "ReferredBy"
            };

            // Act
            _provider.Index(contact);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.CONTACT_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.CONTACT_ID), "Document had an incorrect contact id value");
            Assert.AreEqual("Assistant", doc.Get(LuceneSearchProvider.Constants.CONTACT_ASSISTANT), "Document had an incorrect assistant value");
            Assert.AreEqual("Direct Phone", doc.Get(LuceneSearchProvider.Constants.CONTACT_DIRECTPHONE), "Document had an incorrect direct phone value");
            Assert.AreEqual("Email", doc.Get(LuceneSearchProvider.Constants.CONTACT_EMAIL), "Document had an incorrect email value ");
            Assert.AreEqual("Ext", doc.Get(LuceneSearchProvider.Constants.CONTACT_EXTENSION), "Document had an incorrect extension value");
            Assert.AreEqual("MobilePhone", doc.Get(LuceneSearchProvider.Constants.CONTACT_MOBILEPHONE), "Document had an incorrect mobile phone value");
            Assert.AreEqual("Name", doc.Get(LuceneSearchProvider.Constants.CONTACT_NAME), "Document had an incorrect name value");
            Assert.AreEqual("Notes", doc.Get(LuceneSearchProvider.Constants.CONTACT_NOTES), "Document had an incorrect notes value");
            Assert.AreEqual("ReferredBy", doc.Get(LuceneSearchProvider.Constants.CONTACT_REFERREDBY), "Document had an incorrect referred by value");
        }

        [TestMethod]
        public void Indexed_Contacts_Null_Properties_Index_As_Empty_String()
        {
            // Setup
            Contact contact = new Contact { Id = 3 };

            // Act
            _provider.Index(contact);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.CONTACT_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.CONTACT_ID), "Document had an incorrect contact id value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_ASSISTANT), "Document had an incorrect assistant value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_DIRECTPHONE), "Document had an incorrect direct phone value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_EMAIL), "Document had an incorrect email value ");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_EXTENSION), "Document had an incorrect extension value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_MOBILEPHONE), "Document had an incorrect mobile phone value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_NAME), "Document had an incorrect name value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_NOTES), "Document had an incorrect notes value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.CONTACT_REFERREDBY), "Document had an incorrect referred by value");
        }

        [TestMethod]
        public void Can_Index_Multiple_Contacts()
        {
            // Setup
            Contact contact1 = new Contact { Id = 3, Name = "Name" };
            Contact contact2 = new Contact { Id = 4, Name = "Name" };

            // Act
            _provider.Index(contact1);
            _provider.Index(contact2);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.CONTACT_NAME, "Name", out searcher);

            Assert.AreEqual(2, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        public void Can_Remove_Contact_From_Index()
        {
            // Setup
            Contact contact = new Contact { Id = 3 };
            _provider.Index(contact);

            // Act
            _provider.Remove(contact);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.CONTACT_ID, "3", out searcher);

            Assert.AreEqual(0, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        public void ReIndexing_Contact_Updates_Existing_Contact_Document()
        {
            // Setup
            Contact contact = new Contact { Id = 3, Name = "Name1" };
            _provider.Index(contact);
            contact.Name = "Name2";

            // Act
            _provider.Index(contact);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.CONTACT_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("Name2", doc.Get(LuceneSearchProvider.Constants.CONTACT_NAME), "Document had an incorrect name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Indexing_Null_Contact_Throws_ArgumentNullException()
        {
            // Act
            _provider.Index((Contact)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Removing_Null_Contact_Throws_ArgumentNullException()
        {
            // Act
            _provider.Remove((Contact)null);
        }

        [TestMethod]
        public void Can_Index_Tasks()
        {
            // Setup
            DateTime testdate = DateTime.Now;

            Task task = new Task
            {
                Id = 3,
                Name = "Name",
                TaskDate = testdate
            };

            // Act
            _provider.Index(task);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.TASK_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.TASK_ID), "Document had an incorrect contact id value");
            Assert.AreEqual("Name", doc.Get(LuceneSearchProvider.Constants.TASK_NAME), "Document had an incorrect name value");
        }

        [TestMethod]
        public void Tasks_Null_Properties_Index_As_Empty_String()
        {
            // Setup
            Task task = new Task { Id = 3 };

            // Act
            _provider.Index(task);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.TASK_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("3", doc.Get(LuceneSearchProvider.Constants.TASK_ID), "Document had an incorrect contact id value");
            Assert.AreEqual(string.Empty, doc.Get(LuceneSearchProvider.Constants.TASK_NAME), "Document had an incorrect name value");
        }

        [TestMethod]
        public void Can_Index_Multiple_Tasks()
        {
            // Setup
            Task task1 = new Task { Id = 3, Name = "Name" };
            Task task2 = new Task { Id = 4, Name = "Name" };

            // Act
            _provider.Index(task1);
            _provider.Index(task2);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.TASK_NAME, "Name", out searcher);

            Assert.AreEqual(2, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        public void Can_Remove_Task_From_Index()
        {
            // Setup
            Task task = new Task { Id = 3 };
            _provider.Index(task);

            // Act
            _provider.Remove(task);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.TASK_ID, "3", out searcher);

            Assert.AreEqual(0, hits.scoreDocs.Length, "Incorrect number of documents returned");
        }

        [TestMethod]
        public void ReIndexing_Task_Updates_Existing_Contact_Document()
        {
            // Setup
            Task task = new Task { Id = 3, Name = "Name1" };
            _provider.Index(task);
            task.Name = "Name2";

            // Act
            _provider.Index(task);

            // Verify
            IndexSearcher searcher;
            TopDocs hits = SearchIndex(LuceneSearchProvider.Constants.TASK_ID, "3", out searcher);
            Assert.AreEqual(1, hits.scoreDocs.Length, "Incorrect number of documents returned");

            Document doc = searcher.Doc(hits.scoreDocs[0].doc);
            Assert.AreEqual("Name2", doc.Get(LuceneSearchProvider.Constants.TASK_NAME), "Document had an incorrect name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Indexing_Null_Task_Throws_ArgumentNullException()
        {
            // Act
            _provider.Index((Task)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Removing_Null_Task_Throws_ArgumentNullException()
        {
            // Act
            _provider.Remove((Task)null);
        }

        #endregion

        #region Search Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Search_Throws_ArgumentException_When_Searchstring_Is_Null()
        {
            // Act
            _provider.Search(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Search_Throws_ArgumentException_When_Searchstring_Is_Whitespace()
        {
            // Act
            _provider.Search("  ");
        }

        [TestMethod]
        public void Search_Returns_No_Results_With_No_Match()
        {
            // Setup
            Company company = new Company { Id = 2 };
            Contact contact = new Contact { Id = 3 };
            Task task = new Task { Id = 4 };

            _provider.Index(company);
            _provider.Index(contact);
            _provider.Index(task);

            // Act
            SearchProviderResult result = _provider.Search("Test");

            // Verify
            Assert.IsNotNull(result, "Search provider result was null");
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Search provider result's found company list item count was incorrect");
            Assert.AreEqual(0, result.FoundContactIds.Count, "Search provider result's found contact list item count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Search provider result's found task list item count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_Name()
        {
            // Setup 
            Company company = new Company { Id = 2, Name = "My Name" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("Name");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_Phone()
        {
            // Setup 
            Company company = new Company { Id = 2, Phone = "111-222-3334" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("111-222-3334");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_City()
        {
            // Setup 
            Company company = new Company { Id = 2, City = "Orlando" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("Orlando");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_State()
        {
            // Setup 
            Company company = new Company { Id = 2, State = "Florida" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("Florida");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_Zip()
        {
            // Setup 
            Company company = new Company { Id = 2, Zip = "32804" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("32804");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_MetroArea()
        {
            // Setup 
            Company company = new Company { Id = 2, MetroArea = "Orlando" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("Orlando");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_Industry()
        {
            // Setup 
            Company company = new Company { Id = 2, Industry = "Engineering" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("Engineering");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Company_By_Notes()
        {
            // Setup 
            Company company = new Company { Id = 2, Notes = "This is my note" };
            _provider.Index(company);

            // Act
            SearchProviderResult result = _provider.Search("note");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(company.Id, result.FoundCompanyIds[0], "Found company id was incorrect");

            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_Name()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, Name = "My Name" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("Name");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_Assistant()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, Assistant = "Assistant" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("Assistant");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_DirectPhone()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, DirectPhone = "555-333-2323" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("555-333-2323");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_Email()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, Email = "blah@blah.com" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("blah@blah.com");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_Extension()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, Extension = "23" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("23");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_MobilePhone()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, MobilePhone = "test" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("test");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_Notes()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, Notes = "Notes" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("Notes");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Contact_By_ReferredBy()
        {
            // Setup 
            Contact contact = new Contact { Id = 2, ReferredBy = "ReferredBy" };
            _provider.Index(contact);

            // Act
            SearchProviderResult result = _provider.Search("ReferredBy");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(1, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(contact.Id, result.FoundContactIds[0], "Found contact id value was incorrect");

            Assert.AreEqual(0, result.FoundTaskIds.Count, "Found task count was incorrect");
        }

        [TestMethod]
        public void Search_Finds_Task_By_Name()
        {
            // Setup 
            Task task = new Task { Id = 2, Name = "Name" };
            _provider.Index(task);

            // Act
            SearchProviderResult result = _provider.Search("Name");

            // Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.FoundCompanyIds.Count, "Found company count was incorrect");
            Assert.AreEqual(0, result.FoundContactIds.Count, "Found contact count was incorrect");
            Assert.AreEqual(1, result.FoundTaskIds.Count, "Found task count was incorrect");
            Assert.AreEqual(task.Id, result.FoundTaskIds[0], "Found task id value was incorrect");
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.ViewModels;
using MyJobLeads.DomainModel.Entities;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;

namespace MyJobLeads.DomainModel.Providers.Search
{
    public class LuceneSearchProvider : ISearchProvider
    {
        public LuceneSearchProvider()
        {
            LuceneIndexBaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MyJobLeadsIndex";
        }

        #region Properties

        public string LuceneIndexBaseDirectory { get; protected set; }

        #endregion

        #region Indexing Methods

        public void Index(Company company)
        {
            if (company == null)
                throw new ArgumentNullException("Cannot index a null company");

            // Create a document for the company
            var document = new Document();
            document.Add(new Field(Constants.COMPANY_ID, company.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field(Constants.COMPANY_NAME, company.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_CITY, company.City ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_INDUSTRY, company.Industry ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_METRO, company.MetroArea ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_NOTES, company.Notes ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_PHONE, company.Phone ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_STATE, company.State ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.COMPANY_ZIP, company.Zip ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));

            // Remove any previous documents for the company and add the new one
            Remove(company);

            var writer = OpenIndex();
            try
            {
                writer.AddDocument(document);
                writer.Optimize();
            }
            finally 
            {
                // Make sure the writer attempts to close even if an exception occurs, to prevent stale locks
                writer.Close(); 
            }
        }

        public void Index(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("Cannot index a null company");

            // Create a document for the contact
            var document = new Document();
            document.Add(new Field(Constants.CONTACT_ID, contact.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field(Constants.CONTACT_ASSISTANT, contact.Assistant ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_DIRECTPHONE, contact.DirectPhone ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_EMAIL, contact.Email ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_EXTENSION, contact.Extension ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_MOBILEPHONE, contact.MobilePhone ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_NAME, contact.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_NOTES, contact.Notes ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field(Constants.CONTACT_REFERREDBY, contact.ReferredBy ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));

            // Remove any previous documents for the contact and add the new one
            Remove(contact);

            var writer = OpenIndex();
            try
            {
                writer.AddDocument(document);
                writer.Optimize();
            }
            finally
            {
                // Make sure the writer attempts to close even if an exception occurs, to prevent stale locks
                writer.Close();
            }
        }

        public void Index(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("Can't index a null task");

            // Create a document for the task
            var document = new Document();
            document.Add(new Field(Constants.TASK_ID, task.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field(Constants.TASK_NAME, task.Name ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));

            // Remove any previous documents for the task and add the new one
            Remove(task);

            var writer = OpenIndex();
            try
            {
                writer.AddDocument(document);
                writer.Optimize();
            }
            finally
            {
                // Make sure the writer attempts to close even if an exception occurs, to prevent stale locks
                writer.Close();
            }
        }

        public void Remove(Company company)
        {
            if (company == null)
                throw new ArgumentNullException("Cannot remove the index for a null company");

            // Delete all documents that have a company id matching the passed in company
            var writer = OpenIndex();
            writer.DeleteDocuments(new Term(Constants.COMPANY_ID, company.Id.ToString()));
            writer.Optimize();
            writer.Close();
        }

        public void Remove(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("Can't remove the index for a null contact");

            // Delete all documents that have a matching contact id
            var writer = OpenIndex();
            writer.DeleteDocuments(new Term(Constants.CONTACT_ID, contact.Id.ToString()));
            writer.Optimize();
            writer.Close();
        }

        public void Remove(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("Can't remove the index for a null task");

            // Delete all documents with a matching task id
            var writer = OpenIndex();
            writer.DeleteDocuments(new Term(Constants.TASK_ID, task.Id.ToString()));
            writer.Optimize();
            writer.Close();
        }

        #endregion

        #region Search Methods

        public SearchProviderResult Search(string searchString)
        {
            const int MAX_RESULTS = 10000;

            if (string.IsNullOrWhiteSpace(searchString))
                throw new ArgumentException("Provided search string is empty");

            // Update the search string to make each word allow fuzzyness
            searchString = searchString.Replace("~", string.Empty).Replace(" ", "~ ") + "~";

            // Perform the search
            string[] searchfields = new string[] 
            {
                Constants.COMPANY_CITY, Constants.COMPANY_INDUSTRY, Constants.COMPANY_METRO, Constants.COMPANY_NAME, Constants.COMPANY_NOTES,
                Constants.COMPANY_PHONE, Constants.COMPANY_STATE, Constants.COMPANY_ZIP, Constants.CONTACT_ASSISTANT, Constants.CONTACT_DIRECTPHONE,
                Constants.CONTACT_EMAIL, Constants.CONTACT_EXTENSION, Constants.CONTACT_MOBILEPHONE, Constants.CONTACT_NAME, Constants.CONTACT_NOTES,
                Constants.CONTACT_REFERREDBY, Constants.TASK_NAME
            };
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, searchfields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetDefaultOperator(QueryParser.Operator.AND);

            var query = parser.Parse(searchString);
            var directory = FSDirectory.Open(new DirectoryInfo(LuceneIndexBaseDirectory));
            var searcher = new IndexSearcher(directory, true);
            var hits = searcher.Search(query, MAX_RESULTS);

            // Go through found documents and add them to the result
            var result = new SearchProviderResult();

            foreach(var scoreDoc in hits.scoreDocs)
            {
                string id;

                // Retrieve the document, and determine if it's a company, contact, or task and add it to the correct list
                var doc = searcher.Doc(scoreDoc.doc);

                if ((id = doc.Get(Constants.COMPANY_ID)) != null)
                    result.FoundCompanyIds.Add(Convert.ToInt32(id));

                else if ((id = doc.Get(Constants.CONTACT_ID)) != null)
                    result.FoundContactIds.Add(Convert.ToInt32(id));

                else if ((id = doc.Get(Constants.TASK_ID)) != null)
                    result.FoundTaskIds.Add(Convert.ToInt32(id));
            }

            return result;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Creates an index writer for 
        /// </summary>
        /// <returns></returns>
        public IndexWriter OpenIndex()
        {
            var directory = FSDirectory.Open(new DirectoryInfo(LuceneIndexBaseDirectory));

            // Open the index
            bool createNewIndex = !IndexReader.IndexExists(directory);
            var writer = new IndexWriter(directory, CreateAnalyzer(), createNewIndex, IndexWriter.MaxFieldLength.UNLIMITED);

            return writer;
        }

        /// <summary>
        /// Creates an analyzer for usage by index reader and writers
        /// </summary>
        /// <returns></returns>
        public Analyzer CreateAnalyzer()
        {
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
        }

        #endregion

        #region Constants

        public static class Constants
        {
            // Document field name constants for all searchable fields
            public const string COMPANY_ID = "company_id";
            public const string COMPANY_NAME = "company_name";
            public const string COMPANY_PHONE = "company_phone";
            public const string COMPANY_CITY = "company_city";
            public const string COMPANY_STATE = "company_state";
            public const string COMPANY_ZIP = "company_zip";
            public const string COMPANY_METRO = "company_metroarea";
            public const string COMPANY_INDUSTRY = "company_industry";
            public const string COMPANY_NOTES = "company_notes";

            public const string CONTACT_ID = "contact_id";
            public const string CONTACT_NAME = "contact_name";
            public const string CONTACT_DIRECTPHONE = "contact_directphone";
            public const string CONTACT_MOBILEPHONE = "contact_mobilephone";
            public const string CONTACT_EXTENSION = "contact_extension";
            public const string CONTACT_EMAIL = "contact_email";
            public const string CONTACT_ASSISTANT = "contact_assistant";
            public const string CONTACT_REFERREDBY = "contact_referredby";
            public const string CONTACT_NOTES = "contact_notes";

            public const string TASK_ID = "task_id";
            public const string TASK_NAME = "task_name";
        }

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TinySite.Models
{
    [DebuggerDisplay("DocumentFile: {Id}, Source: {SourceRelativePath}")]
    public class DocumentFile : OutputFile
    {
        private object _lock = new object();

        public DocumentFile(string path, string rootPath, string outputPath, string outputRootPath, string url, string rootUrl, Author author, MetadataCollection metadata, IDictionary<string, string> queries)
            : base(path, rootPath, outputPath, outputRootPath, rootUrl, url)
        {
            this.Author = author;

            var now = DateTime.Now;

            this.Now = now;
            this.NowUtc = now.ToUniversalTime();
            this.NowFriendlyDate = now.ToString("D");
            this.NowStandardUtcDate = now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ");

            if (metadata != null)
            {
                this.Layout = metadata.Get<string>("layout");
                metadata.Remove("layout");

                this.Metadata = metadata;
            }

            this.Queries = queries;
        }

        private DocumentFile(DocumentFile original)
            : base(original)
        {
            this.ExtensionsForRendering = new List<string>(original.ExtensionsForRendering);

            this.Layouts = new List<LayoutFile>(original.Layouts);

            this.Partial = original.Partial;

            this.Metadata = original.Metadata;

            this.Queries = original.Queries;

            this.Unmodified = original.Unmodified;

            this.Cloned = true;
        }

        internal IList<string> ExtensionsForRendering { get; set; }

        internal IEnumerable<LayoutFile> Layouts { get; private set; }

        internal IDictionary<string, string> Queries { get; private set; }

        internal bool Cloned { get; }

        internal bool Partial { get; set; }

        internal bool Rendered { get; set; }

        internal string RenderedContent { get; set; }

        public Author Author { get { return this.Get<Author>(); } set { this.Set<Author>(value); } }

        public string Layout { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public string Content { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public bool Draft { get { return this.Get<bool>(); } set { this.Set<bool>(value); } }

        public string Id { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public int Order { get { return this.Get<int>(); } set { this.Set<int>(value); } }

        public string PaginateQuery { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public string ParentId { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public string SourceContent { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public string Summary { get { return this.Get<string>(); } set { this.Set<string>(value); } }

        public DocumentFile NextDocument { get { return this.Get<DocumentFile>(); } set { this.Set<DocumentFile>(value); } }

        public DocumentFile ParentDocument { get { return this.Get<DocumentFile>(); } set { this.Set<DocumentFile>(value); } }

        public DocumentFile PreviousDocument { get { return this.Get<DocumentFile>(); } set { this.Set<DocumentFile>(value); } }

        public Book Book { get { return this.Get<Book>(); } set { this.Set<Book>(value); } }

        public BookPage Chapter { get { return this.Get<BookPage>(); } set { this.Set<BookPage>(value); } }

        public Paginator Paginator { get { return this.Get<Paginator>(); } set { this.Set<Paginator>(value); } }

        public DateTime Now { get { return this.Get<DateTime>(); } private set { this.Set(value); } }

        public DateTime NowUtc { get { return this.Get<DateTime>(); } private set { this.Set(value); } }

        public string NowFriendlyDate { get { return this.Get<string>(); } private set { this.Set<string>(value); } }

        public string NowStandardUtcDate { get { return this.Get<string>(); } private set { this.Set<string>(value); } }

        public MetadataCollection Metadata { get; private set; }

        public DocumentFile CloneForPage(string urlFormat, string prependPathFormat, Paginator paginator)
        {
            var prependPath = String.Format(prependPathFormat, paginator.Pagination.Page);

            var prependUrl = String.Format(urlFormat, paginator.Pagination.Page);

            var dupe = new DocumentFile(this);

            var updateFileName = Path.GetFileName(dupe.OutputRelativePath);

            dupe.OutputRelativePath = Path.Combine(prependPath, updateFileName);

            dupe.OutputPath = Path.Combine(dupe.OutputRootPath, prependPath, updateFileName);

            dupe.RelativeUrl = String.Concat(prependUrl, updateFileName.Equals("index.html", StringComparison.OrdinalIgnoreCase) ? String.Empty : updateFileName);

            dupe.Paginator = paginator;

            return dupe;
        }

        public void AssignLayouts(IEnumerable<LayoutFile> layouts)
        {
            this.Layouts = layouts.ToList();
        }
    }
}

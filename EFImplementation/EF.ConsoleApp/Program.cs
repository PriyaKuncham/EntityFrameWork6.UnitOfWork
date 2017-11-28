using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EF.ConsoleApp
{
    class Program
    {
        //private readonly 
        static void Main(string[] args)
        {
            Program.ReadBlog();

            //  Program.InsertMultiple();

            Console.ReadLine();
        }
        public static async void ReadBlog()
        {

            BloggingEntities context = new BloggingEntities();

            var UOW = new UnitOfWork<BloggingEntities>(context);

            var items = UOW.GetRepository<Blog>().GetPagedList(b => new { Name = b.BlogId, Link = b.Url });

            foreach (var item in items.Items)
            {
                Console.WriteLine("{0} ---- {1}", item.Link, item.Name);
            }
        }

        public static object PropertyName<T>(Expression<Func<T, object>> property) where T : class
        {
            MemberExpression body = (MemberExpression)property.Body;
            return body.Member;
        }
        public static void InsertMultiple()
        {

            BloggingEntities context = new BloggingEntities();

            var UOW = new UnitOfWork<BloggingEntities>(context);

            BloggingPostsEntities context2 = new BloggingPostsEntities();

            var UOW1 = new UnitOfWork<BloggingPostsEntities>(context2);

            var x = UOW.DbContext.Database.CurrentTransaction;
            var y = UOW1.DbContext.Database.CurrentTransaction;


            using (var trans1 = UOW.checkDbContextTransaction())
            {
                using (var trans2 = UOW1.checkDbContextTransaction())
                {
                    var items = UOW.GetRepository<Blog>().GetPagedList(b => new { Name = b.BlogId, Link = b.Url });

                    foreach (var item in items.Items)
                    {
                        Console.WriteLine("{0} ---- {1}", item.Link, item.Name);
                    }
                    var items1 = UOW1.GetRepository<BlogInfo>().GetPagedList(b => new { Name = b.BlogId, Link = b.Url });

                    foreach (var item in items1.Items)
                    {
                        Console.WriteLine("{0} ---- {1}", item.Link, item.Name);
                    }
                    Blog bl = new Blog();
                    bl.Name = "XYZ";
                    bl.Url = "http://ABC.com/v211/Bindu29";
                    UOW.GetRepository<Blog>().Insert(bl);

                    BlogInfo br = new BlogInfo();
                    br.Name = "ABC";
                    br.Url = "http://ABC.com/v211/Priya29";

                    UOW1.GetRepository<BlogInfo>().Insert(br);


                    UOW.SaveChanges();
                    UOW1.SaveChanges();
                }

            }
        }
    }
}

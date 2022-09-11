using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTester
{
    public class Helper
    {
        public enum Methods
        {
            Get,
            Post,
            Patch,
            Put,
            Delete
        }

        public static void Write(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }

        public static void WriteAction(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
        }

        public static void WriteResults(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
        }

        public static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(msg);
        }

        public static string Read(string question)
        {
            Write(question);
            return Console.ReadLine();
        }

        // 0 : Get
        public static List<Action> Targets0(int count = 512) => new()
            {
                new Action(Methods.Get, $"api/samples/{count}")
            };
        // 1: Get
        public static List<Action> Targets1(int count = 512) => new()
            {
                new Action(Methods.Get, $"api/samples/{count}/cached")
            };

        // 2 : Get
        public static List<Action> Targets2() => new()
            {
                new Action(Methods.Get, "api/orders")
            };

        // 3 : Post
        public static List<Action> Targets3() => new()
            {
                new Action(Methods.Post, "api/orders")
            };

        // 4 : Get with payload
        public static List<Action> Targets4() => new()
            {
                new Action(Methods.Get, "api/orders", true)
            };

        // 5 : Post with payload
        public static List<Action> Targets5() => new()
            {
                new Action(Methods.Post, "api/orders", true)
            };


        // 6 : Get Direct dbContext
        public static List<Action> Targets6() => new()
            {
                new Action(Methods.Get, "api/orders/direct")
            };


        // 7 : Combined Get Direct dbContext
        public static List<Action> Targets7() => new()
            {
                new Action(Methods.Get, "api/orders"),
                new Action(Methods.Get, "api/orders/direct")
            };
    }
}

using System;
using System.Net.NetworkInformation;


// I am using the namespace "System.Net.NetworkInformation" which allows access to network classes, 
// including tools for working with ping requests, network interfaces, IP addresses, etc. 
// Ref: https://learn.microsoft.com/en-us/dotnet/api/system.net.networkinformation?view=net-8.0


class NetworkPerformanceTool
{
    static void Main()
    {
        // ask user to enter number of servers to ping
        Console.Write("How many servers do you want to ping? ");
        int count_users_servers = int.Parse(Console.ReadLine());
        string[] servers = new string[count_users_servers]; // store the servers into an array


        for (int i = 0; i < count_users_servers; i++)
        {
            Console.Write($"Enter server address {i + 1} (e.g., apple.com): ");
            servers[i] = Console.ReadLine();
        }

        // ask user for number of pings to attempt
        Console.Write("Enter the number of ping attempts: ");
        int attempts = int.Parse(Console.ReadLine());


        foreach (var s in servers)
        {
            Console.WriteLine($"\n---- Pinging {s} ---");
            recordPing(s, attempts);
        }
    }



    // method to measure ping metrics
    static void recordPing(string address, int attempts)
    {
        Ping ping = new Ping(); // creates new ping to send ICMP requests

        // initializes all metrics to 0
        int pingSuccess = 0;
        int total_ping_time = 0;
        int pingsFailed = 0;



        for (int i = 1; i <= attempts; i++)
        {
            try
            {
                PingReply reply = ping.Send(address); //sends ICMP ping req to the user's server address

                if (reply.Status == IPStatus.Success)
                {
                    pingSuccess++;
                    total_ping_time += (int)reply.RoundtripTime;
                    Console.WriteLine($"Ping {i}: Time = {reply.RoundtripTime} ms"); // i is server
                }



                else
                {
                    pingsFailed++;
                    Console.WriteLine($"Ping {i}: Request timed out.");
                }
            }


            catch (Exception error)
            {
                pingsFailed++;
                Console.WriteLine($"Ping {i}: Error - {error.Message}");
            }

            System.Threading.Thread.Sleep(500); // adds a delay to ensure pings go through
        }

        showStats(pingSuccess, pingsFailed, total_ping_time, attempts);
    }

    // method to show ping stats
    static void showStats(int successful, int failed, int total_ping_time, int attempts)
    {
        Console.WriteLine("\n---- Ping Statistics ----");
        Console.WriteLine($"Total Attempts: {attempts}");
        Console.WriteLine($"Successful Pings: {successful}");
        Console.WriteLine($"Failed Pings: {failed}");


        if (successful > 0)
        {
            double avgTime = (double)total_ping_time / successful;
            Console.WriteLine($"Average Ping Time: {avgTime} ms");
        }



        else
        {
            Console.WriteLine("No successful pings to calculate avg time.");
        }

        double packetLoss = (double)failed / attempts * 100;
        Console.WriteLine($"Packet Loss: {packetLoss}%");
    }
}


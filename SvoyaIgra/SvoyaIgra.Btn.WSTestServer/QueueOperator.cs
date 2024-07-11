using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SvoyaIgra.Shared.Constants;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Btn.WSTestServer
{
    public static class QueueOperator
    {
        //queue selector
        static bool started = false;

        //memory queue
        static int[] queue = { 0, 0, 0, 0 };

        //queue selector
        static int queueSelector = 0;

        public static void ProcessMessage(string message)
        {
            switch (message)
            {
                case WsMessages.StartCommand:
                    EVH_start();
                    break;
                case WsMessages.NextCommand:
                    EVH_next();
                    break;
                case WsMessages.ResetCommand:
                    EVH_reset();
                    break;
                case WsMessages.RedButton:
                    ButtonPressed((int)ButtonEnum.Red);
                    break;
                case WsMessages.GreenButton:
                    ButtonPressed((int)ButtonEnum.Green);
                    break;
                case WsMessages.BlueButton:
                    ButtonPressed((int)ButtonEnum.Blue);
                    break;
                case WsMessages.YellowButton:
                    ButtonPressed((int)ButtonEnum.Yellow);
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
                
            }
        }

        public static string MakeWsMessage()
        {
            string wsMessage = "";
            wsMessage += (queueSelector + 1);
            for (int i = 0; i < queue.Length; i++)
            {
                wsMessage += ";";
                wsMessage += queue[i];
            }
            return wsMessage;
        }

        //event handler for next button
        private static void EVH_start()
        {
            Console.WriteLine("start>>");

            Console.WriteLine(started ? "Game already started" : "Game started");
            started = true;
        }

        //event handler for next button
        private static void EVH_next()
        {
            Console.WriteLine("next>>");

            if (!started)
            {
                Console.WriteLine("Game has not started yet");
                return;
            }

            if (queueSelector < 3 && queue[queueSelector] != 0)
            {
                //select next in queue
                queueSelector++;
            }
            else if(queueSelector == 3)
            {
                Console.WriteLine("End of the queue");
                queueSelector = 3;
            }
            else if (queue[queueSelector] == 0)
            {
                Console.WriteLine("Nobody else in the queue");
            }
        }

        //event handler for reset button 
        private static void EVH_reset()
        {
            Console.WriteLine("reset>>");

            //reset selector
            queueSelector = 0;
            //clear queue
            for (int i = 0; i < queue.Length; i++)
            {
                queue[i] = 0;
            }
            started = false;
        }

        private static void ButtonPressed(int button)
        {
            if (!started)
            {
                Console.WriteLine("Game has not started yet");
                return;
            }

            // add button to queue
            Add2Queue(button);
        }

        private static void Add2Queue(int button)
        {
            for (int i = 0; i < queue.Length; i++)
            {
                if (queue[i] == button)
                {
                    Console.WriteLine($"Already in the queue. Button: {(ButtonEnum)button}");
                    return;
                }
                if (queue[i] == 0)
                {
                    queue[i] = button;
                    Console.WriteLine($"Added to the queue. Button: {(ButtonEnum)button}");
                    return;
                }
            }
            Console.WriteLine("add2Queue some error");
        }

    }
}

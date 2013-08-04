using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Net;
using Rug.Osc;

namespace Hexapod
{
    class Program
    {
        /// <summary>
        /// OSC sender object used to set servo positions.
        /// </summary>
        static OscSender oscSender;

        /// <summary>
        /// Channel assignment of servos on hexapod (uderside view).
        /// </summary>
        enum Servo
        {
            LeftFrontHip,       /* Channel 1  */
            LeftFrontKnee,      /* Channel 2  */
            LeftMiddleHip,      /* Channel 3  */
            LeftMiddleKnee,     /* Channel 4  */
            LeftBackHip,        /* Channel 5  */
            LeftBackKnee,       /* Channel 6  */
            RightFrontHip,      /* Channel 7  */
            RightFrontKnee,     /* Channel 8  */
            RightMiddleHip,     /* Channel 9  */
            RightMiddleKnee,    /* Channel 10 */
            RightBackHip,       /* Channel 11 */
            RightBackKnee       /* Channel 12 */    
        }

        /// <summary>
        /// Descrete positions of servos.
        /// </summary>
        enum Position
        {
            Forwards,
            Backwards,
            Up,
            Down
        }

        /// <summary>
        /// Corrisponding PWM duty cycle values (as percetentage) for servo positions.  NaN elemnts should never be used.
        /// </summary>
        static float[,] pwmValues = {{ 3.000000f, float.NaN, 6.500000f, float.NaN, 4.000000f, float.NaN, 9.000000f, float.NaN, 8.500000f, float.NaN, 9.0000000f, float.NaN },   /* Hip forwards     */
                                     { 9.000000f, float.NaN, 8.000000f, float.NaN, 10.00000f, float.NaN, 3.000000f, float.NaN, 7.000000f, float.NaN, 4.5000000f, float.NaN },   /* Hip backwards    */
                                     { float.NaN, 4.500000f, float.NaN, 5.000000f, float.NaN, 4.500000f, float.NaN, 4.500000f, float.NaN, 4.500000f, float.NaN, 4.500000f },    /* Knee up          */
                                     { float.NaN, 8.500000f, float.NaN, 8.500000f, float.NaN, 8.500000f, float.NaN, 8.500000f, float.NaN, 8.500000f, float.NaN, 8.500000f }};   /* Knee down        */

        /// <summary>
        /// pause between servo position changes
        /// </summary>
        static int pause = 100;

        /// <summary>
        /// Entry point of program.
        /// </summary>
        /// <param name="args">
        /// Unused.
        /// </param>
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
            Console.WriteLine("Use arrow keys to control hexapod.  Press any other key to exit.");
            try
            {
                oscSender = new OscSender(IPAddress.Parse("169.254.1.1"), 9000);
                oscSender.Connect();
                while (true)
                {
                    // Discard key press buffer
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    // Ready current key press
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        StepForwards();
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        StepBackwards();
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        SpinRight();
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        SpinLeft();
                    }
                    else 
                    {
                        break;
                    }
                }
                oscSender.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Step forwards cycle.
        /// </summary>
        static void StepForwards()
        {
            Console.WriteLine("Step forwards");
            SetServo(Servo.LeftFrontKnee, Position.Up);
            SetServo(Servo.LeftMiddleKnee, Position.Down);
            SetServo(Servo.LeftBackKnee, Position.Up);
            SetServo(Servo.RightFrontKnee, Position.Down);
            SetServo(Servo.RightMiddleKnee, Position.Up);
            SetServo(Servo.RightBackKnee, Position.Down);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Forwards);
            SetServo(Servo.LeftMiddleHip, Position.Backwards);
            SetServo(Servo.LeftBackHip, Position.Forwards);
            SetServo(Servo.RightFrontHip, Position.Backwards);
            SetServo(Servo.RightMiddleHip, Position.Forwards);
            SetServo(Servo.RightBackHip, Position.Backwards);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontKnee, Position.Down);
            SetServo(Servo.LeftMiddleKnee, Position.Up);
            SetServo(Servo.LeftBackKnee, Position.Down);
            SetServo(Servo.RightFrontKnee, Position.Up);
            SetServo(Servo.RightMiddleKnee, Position.Down);
            SetServo(Servo.RightBackKnee, Position.Up);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Backwards);
            SetServo(Servo.LeftMiddleHip, Position.Forwards);
            SetServo(Servo.LeftBackHip, Position.Backwards);
            SetServo(Servo.RightFrontHip, Position.Forwards);
            SetServo(Servo.RightMiddleHip, Position.Backwards);
            SetServo(Servo.RightBackHip, Position.Forwards);
            Thread.Sleep(pause);
        }

        /// <summary>
        /// Step backwards cycle.
        /// </summary>
        static void StepBackwards()
        {
            Console.WriteLine("Step backwards");
            SetServo(Servo.LeftFrontKnee, Position.Up);
            SetServo(Servo.LeftMiddleKnee, Position.Down);
            SetServo(Servo.LeftBackKnee, Position.Up);
            SetServo(Servo.RightFrontKnee, Position.Down);
            SetServo(Servo.RightMiddleKnee, Position.Up);
            SetServo(Servo.RightBackKnee, Position.Down);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Backwards);
            SetServo(Servo.LeftMiddleHip, Position.Forwards);
            SetServo(Servo.LeftBackHip, Position.Backwards);
            SetServo(Servo.RightFrontHip, Position.Forwards);
            SetServo(Servo.RightMiddleHip, Position.Backwards);
            SetServo(Servo.RightBackHip, Position.Forwards);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontKnee, Position.Down);
            SetServo(Servo.LeftMiddleKnee, Position.Up);
            SetServo(Servo.LeftBackKnee, Position.Down);
            SetServo(Servo.RightFrontKnee, Position.Up);
            SetServo(Servo.RightMiddleKnee, Position.Down);
            SetServo(Servo.RightBackKnee, Position.Up);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Forwards);
            SetServo(Servo.LeftMiddleHip, Position.Backwards);
            SetServo(Servo.LeftBackHip, Position.Forwards);
            SetServo(Servo.RightFrontHip, Position.Backwards);
            SetServo(Servo.RightMiddleHip, Position.Forwards);
            SetServo(Servo.RightBackHip, Position.Backwards);
            Thread.Sleep(pause);
        }

        /// <summary>
        /// Spin right cycle.
        /// </summary>
        static void SpinRight()
        {
            Console.WriteLine("Spin right");
            SetServo(Servo.LeftFrontKnee, Position.Up);
            SetServo(Servo.LeftMiddleKnee, Position.Down);
            SetServo(Servo.LeftBackKnee, Position.Up);
            SetServo(Servo.RightFrontKnee, Position.Down);
            SetServo(Servo.RightMiddleKnee, Position.Up);
            SetServo(Servo.RightBackKnee, Position.Down);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Backwards);
            SetServo(Servo.LeftMiddleHip, Position.Forwards);
            SetServo(Servo.LeftBackHip, Position.Backwards);
            SetServo(Servo.RightFrontHip, Position.Backwards);
            SetServo(Servo.RightMiddleHip, Position.Forwards);
            SetServo(Servo.RightBackHip, Position.Backwards);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontKnee, Position.Down);
            SetServo(Servo.LeftMiddleKnee, Position.Up);
            SetServo(Servo.LeftBackKnee, Position.Down);
            SetServo(Servo.RightFrontKnee, Position.Up);
            SetServo(Servo.RightMiddleKnee, Position.Down);
            SetServo(Servo.RightBackKnee, Position.Up);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Forwards);
            SetServo(Servo.LeftMiddleHip, Position.Backwards);
            SetServo(Servo.LeftBackHip, Position.Forwards);
            SetServo(Servo.RightFrontHip, Position.Forwards);
            SetServo(Servo.RightMiddleHip, Position.Backwards);
            SetServo(Servo.RightBackHip, Position.Forwards);
            Thread.Sleep(pause);
        }

        /// <summary>
        /// Spin left cycle.
        /// </summary>
        static void SpinLeft()
        {
            Console.WriteLine("Spin left");
            SetServo(Servo.LeftFrontKnee, Position.Up);
            SetServo(Servo.LeftMiddleKnee, Position.Down);
            SetServo(Servo.LeftBackKnee, Position.Up);
            SetServo(Servo.RightFrontKnee, Position.Down);
            SetServo(Servo.RightMiddleKnee, Position.Up);
            SetServo(Servo.RightBackKnee, Position.Down);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Forwards);
            SetServo(Servo.LeftMiddleHip, Position.Backwards);
            SetServo(Servo.LeftBackHip, Position.Forwards);
            SetServo(Servo.RightFrontHip, Position.Forwards);
            SetServo(Servo.RightMiddleHip, Position.Backwards);
            SetServo(Servo.RightBackHip, Position.Forwards);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontKnee, Position.Down);
            SetServo(Servo.LeftMiddleKnee, Position.Up);
            SetServo(Servo.LeftBackKnee, Position.Down);
            SetServo(Servo.RightFrontKnee, Position.Up);
            SetServo(Servo.RightMiddleKnee, Position.Down);
            SetServo(Servo.RightBackKnee, Position.Up);
            Thread.Sleep(pause);
            SetServo(Servo.LeftFrontHip, Position.Backwards);
            SetServo(Servo.LeftMiddleHip, Position.Forwards);
            SetServo(Servo.LeftBackHip, Position.Backwards);
            SetServo(Servo.RightFrontHip, Position.Backwards);
            SetServo(Servo.RightMiddleHip, Position.Forwards);
            SetServo(Servo.RightBackHip, Position.Backwards);
            Thread.Sleep(pause);
        }

        /// <summary>
        /// Set servo position.
        /// </summary>
        /// <param name="servo">
        /// Servo joint on hexapod.
        /// </param>
        /// <param name="position">
        /// Descrete position of servo.
        /// </param>
        static void SetServo(Servo servo, Position position)
        {
            string channel = ((int)(servo) + 1).ToString();
            float pwmValue = pwmValues[(int)position, (int)servo];
            oscSender.Send(new OscMessage("/output/pwm/duty/" + channel.ToString(), pwmValue));
        }
    }
}

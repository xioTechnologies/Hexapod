Hexapod
=======

A C# application to control sparkfun's [12-servo hexapod ](https://www.sparkfun.com/products/11497) using [x-OSC](http://www.x-io.co.uk/x-osc/).  The arrow keys are used to make the robot walk forwards, backwards, spin clockwise or spin counter clockwise using a tripod gait.  x-OSC's output channels must be 1 to 12 are set to PWM mode with a frequency of 50 Hz.

The application uses the open-source C# OSC library, [Rug.Osc](https://bitbucket.org/rugcode/rug.osc/).

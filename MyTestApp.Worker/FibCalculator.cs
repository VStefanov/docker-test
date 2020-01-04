using System;
using System.Collections.Generic;
using System.Text;

namespace Worker.MyTestApp.Worker
{
    public class FibCalculator
    {
        public int Execute(int index)
        {
            if ((index == 0) || (index == 1))
            {
                return index;
            }
            else
                return Execute(index - 1) + Execute(index - 2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BlockStack
{
    public class CountdownTimer
    {


        private int startCount;
        private int endCount;
        public String displayValue { get; private set; }
        public Boolean isActive { get; private set; }
        public Boolean isComplete { get; private set; }


        public CountdownTimer()
        {
            reset();
        }


        public void setAndStart(GameTime gt, int milliseconds)
        {
            startCount = gt.TotalGameTime.Milliseconds;
            endCount   = gt.TotalGameTime.Milliseconds + Math.Abs(milliseconds);
            isActive   = true;
            isComplete = false;
            displayValue = endCount.ToString();
        }


        public Boolean checkExpired(GameTime gt)
        {
            if (isComplete)
                this.displayValue = "timer over";
            else
            {
                if (endCount-startCount > 0)
                {
                    startCount += gt.TotalGameTime.Milliseconds;
                    displayValue = (endCount-startCount).ToString();
                }
                else
                {
                    isComplete = true;
                    displayValue = "timer over";

                }
            }
            return isComplete;
        }


        public void reset()
        {
            isActive     = false;
            isComplete   = false;
            displayValue = "None";
            startCount   = 0;
            endCount     = 0;
        }
    }
}
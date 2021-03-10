using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAndDelegate
{

    //************************************* 申明委托
    public delegate void BoiledEventHandler(Object sender, BoiledEventArgs e);

    //声明委托中所用到的EventArgs
    // 定义 BoiledEventArgs 类，传递给 Observer 所感兴趣的信息
    public class BoiledEventArgs : EventArgs
    {
        public readonly int temperature;
        public BoiledEventArgs(int temperature)
        {
            this.temperature = temperature;
        }
    }
    //Publisher 发布类
    public class Heater
    {
        private int temperature;
        public string type = "RealFire 001"; // 添加型号作为演示
        public string area = "China Xian"; // 添加产地作为演示

        //************************************* 在发布类中声明事件：对应于委托
        public event BoiledEventHandler Boiled; // 声明事件


        // 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        protected virtual void OnBoiled(BoiledEventArgs e)
        {
            if (Boiled != null)
            {
                //Raise 激发事件
                Boiled(this, e); // 调用所有注册对象的方法
            }
        }

        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;
                if (temperature > 95)
                {
                    // 建立BoiledEventArgs 对象。

                    BoiledEventArgs e = new BoiledEventArgs(temperature);
                    //************************************* 在发布类中Raise 激发事件
                    OnBoiled(e); // 通过调用 OnBolied 方法，在方法中激发事件。这样可以重写方法，以便继承类拒绝其他对象对它的监视
                    //Boiled(this,e); //也可以直接激发事件
                }
            }
        }

        public class Alarm
        {
            public void MakeAlert(Object sender, BoiledEventArgs e)
            {
                Heater heater = (Heater)sender; // 这里是不是很熟悉呢？

                // 访问 sender 中的公共字段
                Console.WriteLine("Alarm：{0} - {1}: ", heater.area, heater.type);
                Console.WriteLine("Alarm: 嘀嘀嘀，水已经 {0} 度了：", e.temperature);
                Console.WriteLine();
            }
        }

        public class Display
        {
            public static void ShowMsg(Object sender, BoiledEventArgs e) // 静态方法
            {
                Heater heater = (Heater)sender;
                Console.WriteLine("Display：{0} - {1}: ", heater.area, heater.type);
                Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", e.temperature);
                Console.WriteLine();
            }
        }


    }
    //Subscriber 订阅类
    class Program
    {
        static void Main()
        {
            Heater heater = new Heater();
            Heater.Alarm alarm = new Heater.Alarm();
            //---------------------------------------注册事件/委托，向委托中添加方法
            heater.Boiled += alarm.MakeAlert; //注册方法
            heater.Boiled += (new Heater.Alarm()).MakeAlert; //给匿名对象注册方法
            heater.Boiled += new BoiledEventHandler(alarm.MakeAlert); //也可以这么注册
            heater.Boiled += Heater.Display.ShowMsg; //注册静态方法
            heater.BoilWater(); //烧水，会自动调用注册过对象的方法
            Console.ReadKey();
        }
    }
}

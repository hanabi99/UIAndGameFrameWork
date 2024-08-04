using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facadeMain : MonoBehaviour
{
    void Start()
    {
        HomeFacade homeTheaterFacade = new HomeFacade();
        homeTheaterFacade.ready();
        homeTheaterFacade.end();
    }


    public class HomeFacade
    {
        //定义各个子系统对象
        private Player player;
        private Screen screen;
        private Stereo stereo;
        private Projector projector;

        //构造器
        public HomeFacade()
        {
            this.player = Player.getInstance();
            this.screen = Screen.getInstance();
            this.stereo = Stereo.getInstance();
            this.projector = Projector.getInstance();
        }

        public void ready()
        {
            Debug.Log("===ready===");
            screen.down();
            player.on();
            stereo.on();
            projector.on();
        }
        public void end()
        {
            Debug.Log("===end===");
            screen.up();
            projector.off();
            stereo.off();
            player.off();
        }

    }

    public class Player
    { //播放器
        private static Player instance = new Player();//使用单例模式(饿汉式)
        public static Player getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" 播放器打开了 ");
        }
        public void off()
        {
            Debug.Log(" 播放器关闭了 ");
        }
        public void play()
        {
            Debug.Log(" 播放器播放中 ");
        }
        public void pause()
        {
            Debug.Log(" 播放暂停 ");
        }
    }

    public class Screen
    { //屏幕
        private static Screen instance = new Screen();
        public static Screen getInstance()
        {
            return instance;
        }
        public void up()
        {
            Debug.Log(" 屏幕上升 ");
        }
        public void down()
        {
            Debug.Log(" 屏幕下降 ");
        }
    }

    public class Stereo
    { //音响
        private static Stereo instance = new Stereo();
        public static Stereo getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" 音响打开了 ");
        }
        public void off()
        {
            Debug.Log(" 音响关闭了 ");
        }
    }

    public class Projector
    { //投影仪
        private static Projector instance = new Projector();
        public static Projector getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" 投影仪打开了 ");
        }
        public void off()
        {
            Debug.Log(" 投影仪关闭了 ");
        }
    }

}

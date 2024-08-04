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
        //���������ϵͳ����
        private Player player;
        private Screen screen;
        private Stereo stereo;
        private Projector projector;

        //������
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
    { //������
        private static Player instance = new Player();//ʹ�õ���ģʽ(����ʽ)
        public static Player getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" ���������� ");
        }
        public void off()
        {
            Debug.Log(" �������ر��� ");
        }
        public void play()
        {
            Debug.Log(" ������������ ");
        }
        public void pause()
        {
            Debug.Log(" ������ͣ ");
        }
    }

    public class Screen
    { //��Ļ
        private static Screen instance = new Screen();
        public static Screen getInstance()
        {
            return instance;
        }
        public void up()
        {
            Debug.Log(" ��Ļ���� ");
        }
        public void down()
        {
            Debug.Log(" ��Ļ�½� ");
        }
    }

    public class Stereo
    { //����
        private static Stereo instance = new Stereo();
        public static Stereo getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" ������� ");
        }
        public void off()
        {
            Debug.Log(" ����ر��� ");
        }
    }

    public class Projector
    { //ͶӰ��
        private static Projector instance = new Projector();
        public static Projector getInstance()
        {
            return instance;
        }
        public void on()
        {
            Debug.Log(" ͶӰ�Ǵ��� ");
        }
        public void off()
        {
            Debug.Log(" ͶӰ�ǹر��� ");
        }
    }

}

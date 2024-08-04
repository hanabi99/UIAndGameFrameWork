using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum VideoType
{
    mp4,
    h264,
    h262
}

public class ProxyMain : MonoBehaviour
{

    void Start()
    {
        VideoDecoderProxy proxy = new VideoDecoderProxy(); //代理对象

        proxy.RegisterVideoDecoder(VideoType.mp4, new MP4VideoDecoder());

            
        proxy.RegisterVideoDecoder(VideoType.h264, new H264VideoDecoder());

        proxy.RegisterVideoDecoder(VideoType.h262, new H262VideoDecoder());

        proxy.Decode(null, VideoType.h262);
    }

    public class VideoDecoderProxy : VideoDecoder
    {
        Dictionary<VideoType, VideoDecoder> dic;

        public VideoDecoderProxy()
        {
            dic = new Dictionary<VideoType, VideoDecoder>();
        }
        public void RegisterVideoDecoder(VideoType videoType, VideoDecoder decoder)
        {
            dic.Add(videoType, decoder);
        }
        // 根据视频的类型，初始化代理的具体解码者

        public RawImage Decode(byte[] videoDatas, VideoType videoType)
        {
            switch (videoType)
            {
                case VideoType.mp4:
                    return dic[VideoType.mp4].Decode(videoDatas, VideoType.mp4);
                case VideoType.h264:
                    return dic[VideoType.h264].Decode(videoDatas, VideoType.h264);
                case VideoType.h262:
                    return dic[VideoType.h262].Decode(videoDatas, VideoType.h262);
                default:
                    return null;
            }
        }

    };

    public class H264VideoDecoder : VideoDecoder
    {
        public RawImage Decode(byte[] videoDatas, VideoType videoType)
        {
            throw new System.NotImplementedException();
        }
    }

    public class H262VideoDecoder : VideoDecoder
    {
        public RawImage Decode(byte[] videoDatas, VideoType videoType)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MP4VideoDecoder : VideoDecoder
    {
        public RawImage Decode(byte[] videoDatas, VideoType videoType)
        {
            throw new System.NotImplementedException();
        }
    }


    public interface VideoDecoder
    {
        RawImage Decode(byte[] videoDatas, VideoType videoType);
    }
}

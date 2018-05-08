using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace SyncFrame
{


	public class SFRecorderActionSyncMachine<T, ActionType, ParamType> : ISFActionSyncMachine<ActionType, ParamType> where ActionType : IComparable where T : SFMgr<T, ActionType, ParamType>
    {
        public T mgr;

        private SFFramesJson<ActionType, ParamType> framesFile = new SFFramesJson<ActionType, ParamType>();

        public SFRecorderActionSyncMachine(T mgr)
        {
            this.mgr = mgr;
        }


        public void DispatchActions(Action onEnd)
        {
            onEnd();
        }

        public void PostAction(SFAction<ActionType, ParamType> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 推送Actions，并构建对应的Json类结构
        /// </summary>
        /// <param name="actions"></param>
        public void PostActions(List<SFAction<ActionType, ParamType>> actions)
        {
            var frame =  SFFrameJson<ActionType, ParamType>.CreateFrame(actions);
            frame.FrameID = mgr.CurFrameID + 1;
            framesFile.Frames.Add(frame);
        }

        /// <summary>
        /// 保存到路径
        /// </summary>
        /// <param name="pathName"></param>
        public void SaveActionsJson(string pathName)
        {
            //string filePath = Application.dataPath + @"/Resources/" + pathName;

            //string strData = JsonUtility.ToJson(framesFile);
            //System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            //System.IO.StreamWriter sw = file.CreateText();
            //sw.WriteLine(strData);
            //sw.Close();
            //sw.Dispose();

            using (var file = File.Create(pathName))
            {
                Serializer.Serialize(file, framesFile);
            }

        }

        public void SetActionsMask()
        {

        }

		public void Dispose()
		{

		}

    }


}


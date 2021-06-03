using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 創建不同種類的Message之介面。
/// </summary>
public interface IMessageCreateType
{
    GameObject InitialMessage(MessageDisplaySetting _mdSetting);
}

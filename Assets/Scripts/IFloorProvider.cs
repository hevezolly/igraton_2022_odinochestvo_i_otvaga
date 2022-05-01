using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorProvider
{
    RenderTexture GetTexture();

    void UpdateTexture();

    Rect GetLocalRect();

    bool CanBeWiped { get; }

    Transform GetTransform();
}

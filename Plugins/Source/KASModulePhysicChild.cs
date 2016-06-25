﻿using UnityEngine;
using System;
using System.Collections;

namespace KAS {

public class KASModulePhysicChild : PartModule {
  public float mass = 0.01f;
  public GameObject physicObj;

  bool physicActive;
  Vector3 currentLocalPos;
  Quaternion currentLocalRot;

  /// <summary>Starts physics handling on the object.</summary>
  /// <remarks>The object is expected to not have Rigidbody. The one will be added with the proper
  /// mass and velocity settings.</remarks>
  public void StartPhysics() {
    KAS_Shared.DebugLog("StartPhysics(PhysicChild)");
    if (!physicActive) {
      var physicObjRigidbody = physicObj.AddComponent<Rigidbody>();
      physicObjRigidbody.mass = mass;
      physicObj.transform.parent = null;
      physicObjRigidbody.useGravity = false;
      physicObjRigidbody.velocity = part.Rigidbody.velocity;
      physicObjRigidbody.angularVelocity = part.Rigidbody.angularVelocity;
      physicActive = true;
    } else {
      KAS_Shared.DebugWarning("StartPhysics(PhysicChild) Physic already started! Ingore.");
    }
  }

  /// <summary>Stops physics handling on the object.</summary>
  /// <remarks>Rigidbody on the object gets destroyed.</remarks>
  public void StopPhysics() {
    KAS_Shared.DebugLog("StopPhysics(PhysicChild)");
    if (physicActive) {
      UnityEngine.Object.Destroy(physicObj.GetComponent<Rigidbody>());
      physicObj.transform.parent = part.transform;
      physicActive = false;
    } else {
      KAS_Shared.DebugWarning("StopPhysics(PhysicChild) Physic already stopped! Ignore.");
    }
  }

  /// <summary>Part's message handler.</summary>
  /// <remarks>Temporarily suspends physics handling on the object.</remarks>
  void OnPartPack() {
    if (physicActive) {
      KAS_Shared.DebugLog("OnPartPack(PhysicChild)");
      currentLocalPos = KAS_Shared.GetLocalPosFrom(physicObj.transform, part.transform);
      currentLocalRot = KAS_Shared.GetLocalRotFrom(physicObj.transform, part.transform);
      physicObj.GetComponent<Rigidbody>().isKinematic = true;
      physicObj.transform.parent = part.transform;
      StartCoroutine(WaitPhysicUpdate());
    }
  }

  /// <summary>Part's message handler.</summary>
  /// <remarks>Resumes physics handling on the object.</remarks>
  void OnPartUnpack() {
    if (physicActive) {
      var physicObjRigidbody = physicObj.GetComponent<Rigidbody>();
      if (physicObjRigidbody.isKinematic) {
        KAS_Shared.DebugLog("OnPartUnpack(PhysicChild)");
        physicObj.transform.parent = null;
        KAS_Shared.SetPartLocalPosRotFrom(
            physicObj.transform, part.transform, currentLocalPos, currentLocalRot);
        physicObjRigidbody.isKinematic = false;
        StartCoroutine(WaitPhysicUpdate());
      }
    }
  }

  /// <summary>Overriden from MonoBehavior.</summary>
  void OnDestroy() {
    KAS_Shared.DebugLog("OnDestroy(PhysicChild)");
    if (physicActive) {
      StopPhysics();
    }
  }

  /// <summary>Overriden from MonoBehavior.</summary>
  void FixedUpdate() {
    if (physicActive) {
      var rb = physicObj.GetComponent<Rigidbody>();
      if (rb != null && !rb.isKinematic) {
        rb.AddForce(part.vessel.graviticAcceleration, ForceMode.Acceleration);
      }
    }
  }

  /// <summary>Aligns position, rotation and velocity of the rigidbody.</summary>
  /// <remarks>The update is delayed till the next fixed update to let game's physics to work.
  /// </remarks>
  /// <returns>Nothing.</returns>
  IEnumerator WaitPhysicUpdate() {
    yield return new WaitForFixedUpdate();
    KAS_Shared.SetPartLocalPosRotFrom(
        physicObj.transform, part.transform, currentLocalPos, currentLocalRot);
    var physicObjRigidbody = physicObj.GetComponent<Rigidbody>();
    if (!physicObjRigidbody.isKinematic) {
      KAS_Shared.DebugLog(
          "WaitPhysicUpdate(PhysicChild) Set velocity to: {0} | angular velocity: {1}",
          part.Rigidbody.velocity, part.Rigidbody.angularVelocity);
      physicObjRigidbody.angularVelocity = part.Rigidbody.angularVelocity;
      physicObjRigidbody.velocity = part.Rigidbody.velocity;
    }
  }
}

}  // namespace

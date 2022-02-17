// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/UsersRegistration.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace UsersRegistrationService {
  public static partial class CertificateUsersRegistrationService
  {
    static readonly string __ServiceName = "UsersRegistration.CertificateUsersRegistrationService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::UsersRegistrationService.UserRequest> __Marshaller_UsersRegistration_UserRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::UsersRegistrationService.UserRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::UsersRegistrationService.UserResponse> __Marshaller_UsersRegistration_UserResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::UsersRegistrationService.UserResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Google.Protobuf.WellKnownTypes.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::UsersRegistrationService.RegisteredUsersResponse> __Marshaller_UsersRegistration_RegisteredUsersResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::UsersRegistrationService.RegisteredUsersResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse> __Method_RegisterUser = new grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterUser",
        __Marshaller_UsersRegistration_UserRequest,
        __Marshaller_UsersRegistration_UserResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse> __Method_UnregisterUser = new grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UnregisterUser",
        __Marshaller_UsersRegistration_UserRequest,
        __Marshaller_UsersRegistration_UserResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse> __Method_UpdateUser = new grpc::Method<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateUser",
        __Marshaller_UsersRegistration_UserRequest,
        __Marshaller_UsersRegistration_UserResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::UsersRegistrationService.RegisteredUsersResponse> __Method_GetRegisteredUsers = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::UsersRegistrationService.RegisteredUsersResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetRegisteredUsers",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_UsersRegistration_RegisteredUsersResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::UsersRegistrationService.UsersRegistrationReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of CertificateUsersRegistrationService</summary>
    [grpc::BindServiceMethod(typeof(CertificateUsersRegistrationService), "BindService")]
    public abstract partial class CertificateUsersRegistrationServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::UsersRegistrationService.UserResponse> RegisterUser(global::UsersRegistrationService.UserRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::UsersRegistrationService.UserResponse> UnregisterUser(global::UsersRegistrationService.UserRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::UsersRegistrationService.UserResponse> UpdateUser(global::UsersRegistrationService.UserRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::UsersRegistrationService.RegisteredUsersResponse> GetRegisteredUsers(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(CertificateUsersRegistrationServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_RegisterUser, serviceImpl.RegisterUser)
          .AddMethod(__Method_UnregisterUser, serviceImpl.UnregisterUser)
          .AddMethod(__Method_UpdateUser, serviceImpl.UpdateUser)
          .AddMethod(__Method_GetRegisteredUsers, serviceImpl.GetRegisteredUsers).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CertificateUsersRegistrationServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_RegisterUser, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(serviceImpl.RegisterUser));
      serviceBinder.AddMethod(__Method_UnregisterUser, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(serviceImpl.UnregisterUser));
      serviceBinder.AddMethod(__Method_UpdateUser, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::UsersRegistrationService.UserRequest, global::UsersRegistrationService.UserResponse>(serviceImpl.UpdateUser));
      serviceBinder.AddMethod(__Method_GetRegisteredUsers, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::UsersRegistrationService.RegisteredUsersResponse>(serviceImpl.GetRegisteredUsers));
    }

  }
}
#endregion

// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Prototypes/DataExchangeProtocol.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Kernel {
  public static partial class ExchangeService
  {
    static readonly string __ServiceName = "DataExchangeProtocol.ExchangeService";

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
    static readonly grpc::Marshaller<global::Kernel.SingleUserRequest> __Marshaller_DataExchangeProtocol_SingleUserRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Kernel.SingleUserRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Kernel.Response> __Marshaller_DataExchangeProtocol_Response = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Kernel.Response.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Kernel.MultipleUsersRequest> __Marshaller_DataExchangeProtocol_MultipleUsersRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Kernel.MultipleUsersRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Kernel.HostRequest> __Marshaller_DataExchangeProtocol_HostRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Kernel.HostRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Kernel.SettingsRequest> __Marshaller_DataExchangeProtocol_SettingsRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Kernel.SettingsRequest.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Kernel.SingleUserRequest, global::Kernel.Response> __Method_RegisterSingleUser = new grpc::Method<global::Kernel.SingleUserRequest, global::Kernel.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterSingleUser",
        __Marshaller_DataExchangeProtocol_SingleUserRequest,
        __Marshaller_DataExchangeProtocol_Response);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Kernel.MultipleUsersRequest, global::Kernel.Response> __Method_RegisterMultipleUsers = new grpc::Method<global::Kernel.MultipleUsersRequest, global::Kernel.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterMultipleUsers",
        __Marshaller_DataExchangeProtocol_MultipleUsersRequest,
        __Marshaller_DataExchangeProtocol_Response);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Kernel.HostRequest, global::Kernel.Response> __Method_RegisterHost = new grpc::Method<global::Kernel.HostRequest, global::Kernel.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterHost",
        __Marshaller_DataExchangeProtocol_HostRequest,
        __Marshaller_DataExchangeProtocol_Response);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Kernel.SettingsRequest, global::Kernel.Response> __Method_GetSettings = new grpc::Method<global::Kernel.SettingsRequest, global::Kernel.Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetSettings",
        __Marshaller_DataExchangeProtocol_SettingsRequest,
        __Marshaller_DataExchangeProtocol_Response);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Kernel.DataExchangeProtocolReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ExchangeService</summary>
    [grpc::BindServiceMethod(typeof(ExchangeService), "BindService")]
    public abstract partial class ExchangeServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Kernel.Response> RegisterSingleUser(global::Kernel.SingleUserRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Kernel.Response> RegisterMultipleUsers(global::Kernel.MultipleUsersRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Kernel.Response> RegisterHost(global::Kernel.HostRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Kernel.Response> GetSettings(global::Kernel.SettingsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ExchangeServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_RegisterSingleUser, serviceImpl.RegisterSingleUser)
          .AddMethod(__Method_RegisterMultipleUsers, serviceImpl.RegisterMultipleUsers)
          .AddMethod(__Method_RegisterHost, serviceImpl.RegisterHost)
          .AddMethod(__Method_GetSettings, serviceImpl.GetSettings).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ExchangeServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_RegisterSingleUser, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Kernel.SingleUserRequest, global::Kernel.Response>(serviceImpl.RegisterSingleUser));
      serviceBinder.AddMethod(__Method_RegisterMultipleUsers, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Kernel.MultipleUsersRequest, global::Kernel.Response>(serviceImpl.RegisterMultipleUsers));
      serviceBinder.AddMethod(__Method_RegisterHost, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Kernel.HostRequest, global::Kernel.Response>(serviceImpl.RegisterHost));
      serviceBinder.AddMethod(__Method_GetSettings, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Kernel.SettingsRequest, global::Kernel.Response>(serviceImpl.GetSettings));
    }

  }
}
#endregion

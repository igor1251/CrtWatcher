// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/HostsRegistration.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace HostsRegistrationService {
  public static partial class ClientHostsRegistrationService
  {
    static readonly string __ServiceName = "HostsRegistration.ClientHostsRegistrationService";

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
    static readonly grpc::Marshaller<global::HostsRegistrationService.ClientHostRegistrationRequest> __Marshaller_HostsRegistration_ClientHostRegistrationRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::HostsRegistrationService.ClientHostRegistrationRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::HostsRegistrationService.ClientHostRegistrationResponse> __Marshaller_HostsRegistration_ClientHostRegistrationResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::HostsRegistrationService.ClientHostRegistrationResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::HostsRegistrationService.ClientHostRegistrationRequest, global::HostsRegistrationService.ClientHostRegistrationResponse> __Method_ProceedRequest = new grpc::Method<global::HostsRegistrationService.ClientHostRegistrationRequest, global::HostsRegistrationService.ClientHostRegistrationResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ProceedRequest",
        __Marshaller_HostsRegistration_ClientHostRegistrationRequest,
        __Marshaller_HostsRegistration_ClientHostRegistrationResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::HostsRegistrationService.HostsRegistrationReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ClientHostsRegistrationService</summary>
    [grpc::BindServiceMethod(typeof(ClientHostsRegistrationService), "BindService")]
    public abstract partial class ClientHostsRegistrationServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::HostsRegistrationService.ClientHostRegistrationResponse> ProceedRequest(global::HostsRegistrationService.ClientHostRegistrationRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ClientHostsRegistrationServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ProceedRequest, serviceImpl.ProceedRequest).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ClientHostsRegistrationServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ProceedRequest, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::HostsRegistrationService.ClientHostRegistrationRequest, global::HostsRegistrationService.ClientHostRegistrationResponse>(serviceImpl.ProceedRequest));
    }

  }
}
#endregion

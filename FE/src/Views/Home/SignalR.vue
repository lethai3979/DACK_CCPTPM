<template>
    <div>
      <h1>SignalR Demo</h1>
      <p>Status: {{ connectionStatus }}</p>
      <button @click="sendMessage">Send Message</button>
      <div>
        <p>Received message: {{ receivedMessage }}</p>
      </div>
    </div>
  </template>
  
  <script>
  import * as signalR from '@microsoft/signalr';
  
  export default {
    data() {
      return {
        connection: null,      // SignalR connection
        connectionStatus: 'Disconnected', // Current connection status
        receivedMessage: ''    // Store received messages
      };
    },
    methods: {
      async initializeSignalR() {
        // Tạo kết nối tới SignalR API server (thay URL tương ứng)
        this.connection = new signalR.HubConnectionBuilder()
          .withUrl("https://your-api-url/hub") // Thay thế bằng URL đúng của API Hub
          .withAutomaticReconnect() // Tự động reconnect khi mất kết nối
          .configureLogging(signalR.LogLevel.Information)
          .build();
  
        // Đăng ký sự kiện nhận tin nhắn từ server (được gọi từ server)
        this.connection.on("ReceiveMessage", (user, message) => {
          this.receivedMessage = `${user}: ${message}`;
        });
  
        // Xử lý kết nối SignalR thành công
        try {
          await this.connection.start();
          this.connectionStatus = 'Connected';
          console.log('SignalR connected');
        } catch (err) {
          console.error('Error while starting connection:', err);
          this.connectionStatus = 'Failed to connect';
        }
      },
  
      sendMessage() {
        if (this.connection) {
          this.connection
            .invoke("SendMessage", "VueClient", "Hello from Vue!")
            .catch(err => console.error(err.toString()));
        }
      }
    },
    
    // Khi component được mount, khởi tạo kết nối SignalR
    async mounted() {
      await this.initializeSignalR();
    },
  
    beforeDestroy() {
      // Khi component bị huỷ, đóng kết nối SignalR
      if (this.connection) {
        this.connection.stop();
      }
    }
  };
  </script>
  

<template>
    <div class="container">
      <div class="row p-1">
        <div class="col-1" style="padding: 10px 20px;">Message</div>
        <div class="col-5">
          <input style="padding: 10px 20px; border-radius: 10px;"
            type="text"
            class="w-100"
            v-model="message"
            @keyup.enter="sendMessage"
          />
        </div>
      </div>
      <div class="row p-1">
        <div class="col-6 text-end">
          <input style="padding: 10px 20px; border-radius: 10px;"
            type="button"
            value="Send Message"
            @click="sendMessage"
            :disabled="!isConnected"
          />
        </div>
      </div>
      <div class="row p-1">
        <div class="col-6">
          <hr />
        </div>
      </div>
      <div class="row p-1">
        <div class="col-6">
          <ul>
            <li v-for="(msg, index) in messages" :key="index">
              {{ msg.user }} : {{ msg.message }}
            </li>
          </ul>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import * as signalR from '@microsoft/signalr'
  
  export default {
    name: 'ChatComponent',
    data() {
      return {
        connection: null,
        message: '',
        messages: [],
        isConnected: false
      }
    },
    async created() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7075/chathub", {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        })
        .configureLogging(signalR.LogLevel.Information)
        .build()
  
      // Xử lý nhận tin nhắn
      this.connection.on("ReceiveMessage", (user, message) => {
        this.messages.push({ user, message })
      })
  
      try {
        await this.connection.start()
        this.isConnected = true
      } catch (err) {
        console.error(err.toString())
      }
    },
    methods: {
      async sendMessage() {
        if (!this.message.trim()) return
  
        try {
          await this.connection.invoke("SendMessage", this.message)
          this.message = ''
        } catch (err) {
          console.error(err.toString())
        }
      }
    },
    beforeDestroy() {
      if (this.connection) {
        this.connection.stop()
      }
    }
  }
  </script>
  
  <style scoped>
  ul {
    list-style-type: none;
    padding: 0;
  }
  
  li {
    margin: 5px 0;
  }
  </style>
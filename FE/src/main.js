import { createApp } from 'vue'
import App from './App.vue'
import router from './router/index.js'
import {Drawer, Button, message } from 'ant-design-vue';
import 'ant-design-vue/dist/reset.css';
import './index.css'
import 'bootstrap/dist/css/bootstrap.min.css';
// Import Bootstrap JS (tùy chọn)
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
const app = createApp(App);
app.use(router);
app.use(Button);
app.use(Drawer);
app.mount('#app');

app.config.globalProperties.$message = message;
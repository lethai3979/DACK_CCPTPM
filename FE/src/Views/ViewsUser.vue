<template>
    
    <div class="container" style="margin-bottom:10px;margin-top:10px;">
        <router-view></router-view>
    </div>

</template>

<script setup>

import { ref } from 'vue';
import { RouterView } from 'vue-router';
import '../style.css';
import Test from '../Views/Home/test.vue';
</script>

<style>
    .footer{
    background-color: #333;

}
</style>
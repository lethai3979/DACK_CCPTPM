<template>
  <!-- <div class="Views">Đây là view của file Views -->
  <!-- </div> -->
  <div class="container">
        <router-view></router-view>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import { RouterView } from 'vue-router';
</script>

<style>

</style>
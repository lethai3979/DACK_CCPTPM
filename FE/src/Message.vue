<template>
  <div v-if="show" class="toast align-items-center text-bg-primary border-0 fade show">
    <div class="d-flex">
      <div class="toast-body">
        {{ message }}
      </div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" @click="close"></button>
    </div>
  </div>
  
</template>

<script>


export default{
  
  data () {
    return {
        message: "",
        show:false
    }
  },
  methods: {
    ShowMess(mess){
        this.show = true;
        this.message = mess;
        const abc = this;
        setTimeout(function () {
                abc.show = false
            }, 2000);
    },
    close(){
      this.show = false;
    }
  },

}
</script>

<style>

</style>
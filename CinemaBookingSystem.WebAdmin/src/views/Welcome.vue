<template>
<div class="container page">
    <div class="row">
        <div class="xs-12 md-6 mx-auto">
            <div id="text-container">
                <div class="number">MyCinema</div>
                <div class="text">Admin Panel</div>
                <div class="text">Probably you not logged in system. Please click button below.</div>
                <div class="text"> <Button @click="onLogin()">Go to login page</Button></div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
import Button from "primevue/button"
export default {
  name: 'WelcomePage',
  props: {
    err: {
      type: String
  }
  },
  components: {
    Button
  },
  methods: {
    onLogin () {
      this.$auth.login()
    },
  }
}
</script>

<style scoped>
p {
    font-weight: bold;
    font-size: 50px;
    text-align: center;
    color: #000000;
}
.page {
min-height: 100vh;
min-height: 100vh;
color: rgba(0, 0, 0, 0.87);
}

.mx-auto {
    margin-left: auto;
    margin-right: auto;
}
#text-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
}
#text-container {
  margin-top: 120px;
}
#text-container .number {
  font-size: 4rem;
  font-weight: 500;
}
#text-container .text {
      margin: 0 0 1rem;
}
#text-container .text {
  font-weight: 300;
  text-align: center;
}
</style>

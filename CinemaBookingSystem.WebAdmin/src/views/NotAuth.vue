<template>
<div class="container page">
    <div class="row">
        <div class="xs-12 md-6 mx-auto">
            <div id="text-container">
                <div class="number">401</div>
                <div class="text">Page not authorized</div>
                <div class="text">This may not mean anything.</div>
                <div class="text" v-if="this.err != null">Error message: {{this.err}}</div>
            </div>
        </div>
    </div>
</div>
</template>

<script>
export default {
  name: 'NotFound',
  props: {
    err: {
      type: String
  }
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

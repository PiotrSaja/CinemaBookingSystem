<template>
  <v-main>
    <v-container fluid>
      <slot></slot>
    </v-container>
  </v-main>
</template>

<script>
export default {
  name: "ContainerComponent"
}
</script>

<style scoped>
.v-main {
  padding-top: 12px !important;
}
</style>

<template>
  <div id="app" v-cloak>
    <b-container>
        <b-row>
          <b-col>
            <navbar :key="$route.fullPath"/>
          </b-col>
        </b-row>
        <b-row>
          <b-col>
            <router-view/>
          </b-col>
        </b-row>
    </b-container>
    <mfooter/>
  </div>
</template>

<script>
import Navbar from '@/components/Navbar'
import mfooter from '@/components/Footer'
export default {
  name: 'App',
  components: {
    Navbar,
    mfooter
  }
}
</script>

<style scoped>
[v-cloak] {display: none}
</style>

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
            <transition name="fade">
              <router-view/>
            </transition>
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

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>

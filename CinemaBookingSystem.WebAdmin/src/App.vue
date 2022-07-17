<template>
  <div id="app" v-cloak>
    <section class="layout">
      <div class="sidebar"><navbar/></div>
      <div class="body">
        <div class="row">
          <menubar1/>
        </div>
        <div class="row">
          <router-view/>
        </div>
        
      </div>
    </section>
  </div>
</template>


<script>
import Navbar from './components/Navbar.vue';
import Menubar1 from './components/Menubar.vue';
export default {
  name: 'App',
  components: {
    Navbar,
    Menubar1
}
}
</script>

<style scoped>
[v-cloak] {display: none}
.layout {
  width: 100%;
  display: grid;
  grid:
    "sidebar body" 1fr
    / 15% 85%;
  gap: 8px;
}
.sidebar { 
  grid-area: sidebar;
  min-height: 100vh;
  border-right: 1px solid lightgray;
 }
.body { grid-area: body; }

#app {
  font-family: 'Lobster', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

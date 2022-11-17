
<template>
  <section class="layout">
    <div class="sidebar"><Navbar /></div>
    <div class="body">
      <div class="row">
        <Menubar1 />
      </div>
      <div class="row p-2">
        <Card v-if="isUserLoggedIn">
          <template #title> Profile </template>
          <template #content> Email : {{ profile.email }} </template>
        </Card>
      </div>
    </div>
  </section>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Menubar1 from "@/components/Menubar.vue";
import Card from "primevue/card";
export default {
  name: "ProfileView",
  data() {
    return {
      isUserLoggedIn: false,
      profile: null,
    };
  },
  components: {
    Card,
    Navbar,
    Menubar1,
  },
  created() {
    this.$auth
      .isUserLoggedIn()
      .then((isLoggedIn) => {
        this.isUserLoggedIn = isLoggedIn;
      })
      .catch((error) => {
        console.log(error);
        this.isUserLoggedIn = false;
      });
    this.$auth
      .getProfile()
      .then((profile) => {
        this.profile = profile;
      })
      .catch((error) => {
        console.log(error);
        this.profile = {};
      });
  },
  methods: {},
};
</script>

<style scoped>
[v-cloak] {
  display: none;
}
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
.body {
  grid-area: body;
}
</style>

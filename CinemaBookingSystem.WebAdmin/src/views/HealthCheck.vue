<template>
  <section class="layout">
    <div class="sidebar"><Navbar /></div>
    <div class="body">
      <div class="row">
        <Menubar1 />
      </div>
      <div class="row p-2">
        <Message severity="success" :closable="false"
          ><h3>Health check</h3>
          Status: {{ healthCheck }}</Message
        >
      </div>
    </div>
  </section>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Menubar1 from "@/components/Menubar.vue";
import HealthCheckService from "@/services/health-check-service";
import Message from "primevue/message";
export default {
  name: "HealthCheckView",
  data() {
    return {
      healthCheck: null,
    };
  },
  components: {
    Message,
    Navbar,
    Menubar1,
  },
  created() {
    HealthCheckService.get()
      .then((response) => {
        this.healthCheck = response.data;
        this.loading = false;
      })
      .catch((error) => {
        console.log(error.response.data);
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

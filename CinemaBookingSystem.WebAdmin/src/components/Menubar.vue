<template>
<Menubar :model="items" style="background: none; border: none">
</Menubar>
</template>

<script>
import Menubar from 'primevue/menubar';
export default {
  name: "MenubarComponent",
  components: {
    Menubar,
 },
  data() {
		return {
			items: [
               {
                   label:'Profile',
                   icon:'pi pi-fw pi-user',
                   url: '/profile'
                },
                {
                   label:'Logout',
                   icon:'pi pi-fw pi-power-off',
                   url: '/profile/logout'
                }
             ]
		}
  },
  created() {
  },
  methods: {
        expandNode(node) {
            if (node.items && node.items.length) {
                this.expandedKeys[node.key] = true;

                for (let child of node.items) {
                    this.expandNode(child);
                }
            }
        }
    }
}
</script>

<style>
.menubar {
   background: none;
}
</style>

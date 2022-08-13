<template>
<nav class="text-center">
    <h2>Cinema System</h2>
    <span>Admin Panel</span>
    <PanelMenu :model="items" v-model:expandedKeys="expandedKeys" class="mr-2 mt-3"/>
</nav>
</template>

<script>
import PanelMenu from 'primevue/panelmenu';
export default {
  name: "NavbarComponent",
  components: {
    PanelMenu,
 },
  data() {
		return {
            expandedKeys: {},
			items: [{
                    key: '0',
                    label: 'General',
                    icon: 'pi pi-fw pi-th-large',
                    items: [
                        {
                            key: '0_1',
                            label: 'Dashboard',
                            icon: 'pi pi-fw pi-home',
                            url: '/home'
                        },
                        {
                            key: '0_2',
                            label: 'Statistic',
                            icon: 'pi pi-fw pi-percentage',
                            url: '/statistic'
                        },
                        {
                            key: '0_3',
                            label: 'Status',
                            icon: 'pi pi-fw pi-info-circle',
                            url: '/hc'
                        }
                    ]
                },
                {
                    key: '1',
                    label: 'Edit',
                    icon: 'pi pi-fw pi-pencil',
                    items: [{
                            key: '1_0',
                            label: 'Bookings',
                            icon: 'pi pi-fw pi-book',
                            items: [{
                                    key: '1_0_0',
                                    label: 'New',
                                    icon: 'pi pi-fw pi-plus',
                                    url: '/bookings/new'
                                },
                                {
                                    key: '1_0_1',
                                    label: 'List',
                                    icon: 'pi pi-fw pi-list',
                                    url: '/bookings'
                                }
                            ]
                        },
                        {
                            key: '1_1',
                            label: 'Cinemas',
                            icon: 'pi pi-fw pi-map-marker',
                            items: [{
                                    key: '1_1_0',
                                    label: 'New',
                                    icon: 'pi pi-fw pi-plus',
                                    url: '/cinemas/new'
                                },
                                {
                                    key: '1_1_1',
                                    label: 'List',
                                    icon: 'pi pi-fw pi-list',
                                    url: '/cinemas'
                                }
                            ]
                        },
                        {
                            key: '1_2',
                            label: 'Movies',
                            icon: 'pi pi-fw pi-play',
                            items: [{
                                    key: '1_2_0',
                                    label: 'New',
                                    icon: 'pi pi-fw pi-plus',
                                    url: '/movies/new'
                                },
                                {
                                    key: '1_2_1',
                                    label: 'List',
                                    icon: 'pi pi-fw pi-list',
                                    url: '/movies'
                                }
                            ]
                        },
                        {
                            key: '1_3',
                            label: 'Seances',
                            icon: 'pi pi-fw pi-video',
                            items: [{
                                    key: '1_3_0',
                                    label: 'New',
                                    icon: 'pi pi-fw pi-plus',
                                    url: '/seances/new'
                                },
                                {
                                    key: '1_3_1',
                                    label: 'List',
                                    icon: 'pi pi-fw pi-list',
                                    url: '/seances'
                                }
                            ]
                        }
                    ]
                },
                {
                    key: '2',
                    label: 'Payments',
                    icon: 'pi pi-fw pi-wallet',
                    items: [
                        {
                            key: '2_0',
                            label: 'List',
                            icon: 'pi pi-fw pi-list',
                            url: '/payments'
                        }
                    ]
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

<style scoped>
/deep/ .p-panelmenu .p-panelmenu-header > a {
    padding: 1rem;
    border: none;
    color: #42001c;
    background: none;
    font-weight: 650;
    border-radius: 3px;
    transition: box-shadow 0.2s;
}
/deep/ .p-panelmenu .p-panelmenu-content {
    padding: 0.25rem 0;
    border: none;
    background: #ffffff;
    color: #495057;
    border-top: 0;
    border-top-right-radius: 0;
    border-top-left-radius: 0;
    border-bottom-right-radius: 3px;
    border-bottom-left-radius: 3px;
}
</style>
<template>
    <div>
        <a :href="'/cinemas/' + cinemaId"><Button class="p-button-raised p-button-rounded mr-3">Cinema information</Button></a>
        <a :href="''"><Button class="p-button-raised p-button-rounded mr-3" disabled>Halls</Button></a>
        <br>
        <Button label="Add new hall" class="p-button-success p-button-rounded mt-3" @click="addNewHall()" />
        <DataTable :value="cinemaHalls" :paginator="true" class="p-datatable-customers mt-3" :rows="10"
            dataKey="id" :rowHover="true" v-model:selection="selectedCinemaHalls" v-model:filters="filters" filterDisplay="menu" :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[10,25,50]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
            :globalFilterFields="['name','numberOfColumns','numberOfRows', 'totalSeats']" responsiveLayout="scroll">
            <template #header>
                <div class="flex justify-content-left align-items-center">
                    <h5 class="m-0">Halls</h5>
                    <span class="p-input-icon-left ml-5">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" />
                    </span>
                </div>
            </template>
            <template #empty>
                No cinema halls found.
            </template>
            <template #loading>
                Loading cinema halls data. Please wait.
            </template>
            <Column field="name" header="Name" sortable style="min-width: 14rem">
                <template #body="{data}">
                   <span>{{data.name}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by name"/>
                </template>
            </Column>
            <Column field="totalSeats" header="Total seats" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.totalSeats}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by city"/>
                </template>
            </Column>
            <Column field="numberOfColumns" header="Number of Columns" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.numberOfColumns}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by street"/>
                </template>
            </Column>
             <Column field="numberOfRows" header="Number of Rows" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.numberOfRows}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by street"/>
                </template>
            </Column>
            <Column headerStyle="width: 3rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-users" @click="showCinemaHallSeats(data.id)"></Button> 
                </template>
            </Column>
            <Column headerStyle="width: 3rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-pencil" @click="showCinemaHallDetail(data.id)"></Button>
                </template>
            </Column>
        </DataTable>
	</div>
</template>

<script>
import {FilterMatchMode, FilterOperator} from 'primevue/api';
import CinemaHallService from '@/services/cinema-hall-service';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import { useRoute } from 'vue-router';
export default {
    name: 'CinemaHallsView',
    components: {
        DataTable,
        Column,
        Button,
        InputText
    },
    data() {
        return {
            cinemaHalls: null,
            selectedCinemaHalls: null,
            filters: {
                'global': {value: null, matchMode: FilterMatchMode.CONTAINS},
                'name': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'totalSeats': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'numberOfColumns': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'numberOfRows': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]}
            },
            loading: true,
            cinemaId: null
        }
    },
    productService: null,
     created () {
        const route = useRoute(); 
        this.cinemaId = route.params.id;
        CinemaHallService.getInCinema(this.cinemaId).then((response) => {
        this.cinemaHalls = response.data.items
        this.loading = false
        }).catch((error) => {
        console.log(error.response.data)
        })
    },
    methods: {
        initFilters() {
            this.filters = {
                'global': {value: null, matchMode: FilterMatchMode.CONTAINS},
            }
        },
        showCinemaHallDetail(cinemaHallId) {
            this.$router.push({name: 'CinemaHall', params: {cinemaId: this.cinemaId, id: cinemaHallId}})
        },
        showCinemaHallSeats(cinemaHallId) {
            this.$router.push({name: 'CinemaHall', params: {cinemaId: this.cinemaId, id: cinemaHallId}})
        },
        addNewHall() {
            this.$router.push({name: 'NewCinemaHall', params: {cinemaId: this.cinemaId}})
        },
    }
}
</script>

<style lang="scss"  scoped>
::v-deep(.p-paginator) {
    .p-paginator-current {
        margin-left: auto;
    }
}

::v-deep(.p-progressbar) {
    height: .5rem;
    background-color: #D8DADC;

    .p-progressbar-value {
        background-color: #607D8B;
    }
}

::v-deep(.p-datepicker) {
    min-width: 25rem;

    td {
        font-weight: 400;
    }
}

::v-deep(.p-datatable.p-datatable-customers) {
    .p-datatable-header {
        padding: 1rem;
        text-align: left;
        font-size: 1.5rem;
    }

    .p-paginator {
        padding: 1rem;
    }

    .p-datatable-thead > tr > th {
        text-align: left;
    }

    .p-datatable-tbody > tr > td {
        cursor: auto;
    }

    .p-dropdown-label:not(.p-placeholder) {
        text-transform: uppercase;
    }
}
a {
    text-decoration: none;
    color: black;
}
</style>
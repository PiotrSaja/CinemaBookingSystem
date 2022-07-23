<template>
    <div>
        <DataTable :value="cinemas" :paginator="true" class="p-datatable-customers" :rows="10"
            dataKey="id" :rowHover="true" v-model:selection="selectedCinemas" v-model:filters="filters" filterDisplay="menu" :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[10,25,50]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
            :globalFilterFields="['name','city','street']" responsiveLayout="scroll">
            <template #header>
                 <div class="flex justify-content-left align-items-center">
                    <h5 class="m-0">Cinemas</h5>
                    <span class="p-input-icon-left ml-5">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" />
                    </span>
                 </div>
            </template>
            <template #empty>
                No cinemas found.
            </template>
            <template #loading>
                Loading cinemas data. Please wait.
            </template>
            <Column field="name" header="Name" sortable style="min-width: 14rem">
                <template #body="{data}">
                   <span>{{data.name}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by name"/>
                </template>
            </Column>
            <Column field="city" header="City" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.city}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by city"/>
                </template>
            </Column>
            <Column field="street" header="Street" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.street}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by street"/>
                </template>
            </Column>
            <Column headerStyle="width: 4rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-pencil" @click="showCinemaDetail(data.id)"></Button>
                </template>
            </Column>
        </DataTable>
	</div>
</template>

<script>
import {FilterMatchMode, FilterOperator} from 'primevue/api';
import CinemaService from '@/services/cinema-service';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
export default {
    name: 'CinemasView',
    components: {
        DataTable,
        Column,
        Button,
        InputText
    },
    data() {
        return {
            cinemas: null,
            selectedCinemas: null,
            filters: {
                'global': {value: null, matchMode: FilterMatchMode.CONTAINS},
                'name': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'city': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'street': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]}
            },
            loading: true
        }
    },
    productService: null,
     created () {
        CinemaService.getAll().then((response) => {
        this.cinemas = response.data.items
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
        showCinemaDetail(cinemaId) {
            this.$router.push({name: 'Cinema', params: {id: cinemaId}})
        }
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
</style>
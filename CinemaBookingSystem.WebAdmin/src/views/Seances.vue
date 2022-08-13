<template>
    <div>
        <DataTable :value="seances" :paginator="true" class="p-datatable-customers" :rows="10"
            dataKey="id" :rowHover="true" v-model:selection="selectedSeances" v-model:filters="filters" filterDisplay="menu" :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[10,25,50]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
            :globalFilterFields="['movie.title','movie.language', 'date', 'cinemaHall.name']" responsiveLayout="scroll">
            <template #header>
                 <div class="flex justify-content-left align-items-center">
                    <h5 class="m-0">Seances</h5>
                    <span class="p-input-icon-left ml-5">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" />
                    </span>
                 </div>
            </template>
            <template #empty>
                No seances found.
            </template>
            <template #loading>
                Loading seances data. Please wait.
            </template>
            <Column field="title" header="Title" sortable style="min-width: 14rem">
                <template #body="{data}">
                    {{data.movie.title}}
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by title"/>
                </template>
            </Column>
            <Column field="language" header="Language" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.movie.language}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by language"/>
                </template>
            </Column>
            <Column field="cinema-hall" header="Cinema Hall" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.cinemaHall.name}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by cinema hall"/>
                </template>
            </Column>
             <Column field="date" header="Date" dataType="date" style="min-width: 8rem">
                <template #body="{data}">
                    {{formatDate(new Date(Date.parse(data.date)))}}
                </template>
            </Column>
            <Column header="Completed" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.isCompleted}}</span>
                </template>
            </Column>
            <Column headerStyle="width: 3rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-users" @click="showSeanceSeats(data.id)" v-if="!data.isCompleted"></Button> 
                </template>
            </Column>
            <Column headerStyle="width: 4rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-pencil" @click="showSeanceDetail(data.id)"></Button>
                </template>
            </Column>
        </DataTable>
	</div>
</template>

<script>
import {FilterMatchMode, FilterOperator} from 'primevue/api';
import SeanceService from '@/services/seance-service';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
export default {
    name: 'SeancesView',
    components: {
        DataTable,
        Column,
        Button,
        InputText,
    },
    data() {
        return {
            seances: null,
            selectedSeances: null,
            filters: {
                'global': {value: null, matchMode: FilterMatchMode.CONTAINS},
                'title': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'language': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'cinema-hall': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
            },
            loading: true,
        }
    },
     created () {
        SeanceService.getAll(1, 1000000).then((response) => {
        this.seances = response.data.items
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
        formatDate(value) {
            return value.toLocaleDateString('en-US', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: 'numeric',
                minute: 'numeric'
            });
        },
        showSeanceDetail(seanceId) {
            this.$router.push({name: 'SeanceDetail', params: {id: seanceId}})
        },
        showSeanceSeats(seanceId) {
            this.$router.push({name: 'SeanceSeats', params: {id: seanceId}})
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
    .movie-image {
    width: 150px;
    box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}
}
</style>
<template>
    <div>
        <DataTable :value="bookings" :paginator="true" class="p-datatable-customers" :rows="10"
            dataKey="id" :rowHover="true" v-model:selection="selectedBookings" v-model:filters="filters" filterDisplay="menu" :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[10,25,50]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
            :globalFilterFields="['personalName.firstName','personalName.lastName', 'createdDate', 'bookingStatus']" responsiveLayout="scroll">
            <template #header>
                 <div class="flex justify-content-left align-items-center">
                    <h5 class="m-0">Bookings</h5>
                    <span class="p-input-icon-left ml-5">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" />
                    </span>
                 </div>
            </template>
            <template #empty>
                No bookings found.
            </template>
            <template #loading>
                Loading bookings data. Please wait.
            </template>
            <Column field="userName" header="User GUID" sortable style="min-width: 14rem">
                <template #body="{data}">
                    {{data.userId}}
                </template>
            </Column>
            <Column field="firstName" header="First name" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.personalName.firstName}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by first name"/>
                </template>
            </Column>
            <Column field="lastName" header="Last name" sortable filterMatchMode="contains" style="min-width: 14rem">
                <template #body="{data}">
                    <span>{{data.personalName.lastName}}</span>
                </template>
                <template #filter="{filterModel}">
                    <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by last name"/>
                </template>
            </Column>
             <Column field="date" header="Created date" sortable dataType="date" style="min-width: 8rem">
                <template #body="{data}">
                    {{formatDate(new Date(Date.parse(data.createdDate)))}}
                </template>
                <template #filter="{filterModel}">
                    <Calendar v-model="filterModel.value" dateFormat="mm/dd/yy" placeholder="mm/dd/yyyy" />
                </template>
            </Column>
            <Column field="status" header="Status" sortable :filterMenuStyle="{'width':'14rem'}" style="min-width: 10rem">
                <template #body="{data}">
                    <span>{{data.bookingStatus}}</span>
                </template>
                <template #filter="{filterModel}">
                    <Dropdown v-model="filterModel.value" :options="statuses" placeholder="Any" class="p-column-filter" :showClear="true">
                        <template #value="slotProps">
                            <span>{{slotProps.value}}</span>
                        </template>
                        <template #option="slotProps">
                            <span>{{slotProps.option}}</span>
                        </template>
                    </Dropdown>
                </template>
            </Column>
            <Column headerStyle="width: 4rem; text-align: center" bodyStyle="text-align: center; overflow: visible">
                <template #body="{data}">
                    <Button type="button" icon="pi pi-info" @click="showBookingDetail(data.id)"></Button>
                </template>
            </Column>
        </DataTable>
	</div>
</template>

<script>
import {FilterMatchMode, FilterOperator} from 'primevue/api';
import BookingService from '@/services/booking-service';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Calendar from 'primevue/calendar';
import Dropdown from 'primevue/dropdown';
export default {
    name: 'BookingsView',
    components: {
        DataTable,
        Column,
        Button,
        InputText,
        Calendar,
        Dropdown
    },
    data() {
        return {
            bookings: null,
            selectedBookings: null,
            filters: {
                'global': {value: null, matchMode: FilterMatchMode.CONTAINS},
                'firstName': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'lastName': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.STARTS_WITH}]},
                'status': {operator: FilterOperator.OR, constraints: [{value: null, matchMode: FilterMatchMode.EQUALS}]},
                'date': {operator: FilterOperator.AND, constraints: [{value: null, matchMode: FilterMatchMode.DATE_IS}]},
            },
            loading: true,
            statuses: [
                'unqualified', 'qualified', 'new', 'negotiation', 'renewal', 'proposal'
            ]
        }
    },
     created () {
        BookingService.getAll(1, 1000000).then((response) => {
        this.bookings = response.data.items
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
        showBookingDetail(bookingId) {
            this.$router.push({name: 'Booking', params: {id: bookingId}})
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
    .movie-image {
    width: 150px;
    box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}
}
</style>
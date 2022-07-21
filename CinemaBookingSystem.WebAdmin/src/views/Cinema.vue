<template>
    <div> 
        <TabView>
            <TabPanel header="General">
                <div class="card mt-2 text-left">
            <h2>Please insert data</h2>
            <h4>General information:</h4>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="Cinema name" />
                    </div>
                </div>  
            </div>
            <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-globe"></i>
                        </span>
                        <InputText placeholder="Url" />
                    </div>
            </div>  
            <h4 class="mt-5">Address:</h4>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="Street" />
                    </div>
                </div>  
            </div>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="City" />
                    </div>
                </div>  
            </div>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="Zip Code" />
                    </div>
                </div>  
            </div>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="State" />
                    </div>
                </div>  
            </div>
            <div class="row">
                <div class="col-12 md:col-4">
                    <div class="p-inputgroup">
                        <span class="p-inputgroup-addon">
                            <i class="pi pi-building"></i>
                        </span>
                        <InputText placeholder="Country" />
                    </div>
                </div>  
            </div>
            <div class="row ml-2 mt-3">
                <Button label="Submit" icon="pi pi-check" iconPos="right" class="p-button-success mr-2"  />
                <Button label="Cancel" icon="pi pi-times" iconPos="right" class="p-button-info mr-2" />
                <Button label="Delete" icon="pi pi-trash" @click="openConfirmation" class="p-button-danger" />
                <Dialog header="Delete" v-model:visible="displayConfirmation" :style="{width: '350px'}" :modal="true">
                    <div class="confirmation-content">
                        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
                        <span>Are you sure you want to delete this ?</span>
                    </div>
                    <template #footer>
                        <Button label="No" icon="pi pi-times" @click="closeConfirmation" class="p-button-text"/>
                        <Button label="Yes" icon="pi pi-check" @click="closeConfirmation" class="p-button-text" autofocus />
                    </template>
                </Dialog>
            </div>
        </div>
            </TabPanel>
            <TabPanel header="Halls">
                <div class="card">
                    <Button label="Add" icon="pi pi-plus" @click="openAddHall" class="p-button-success" />
                    <Dialog header="Add" v-model:visible="displayAddHall" :style="{width: '350px'}" :modal="true">
                        <div class="confirmation-content">
                            <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
                            <span>ADD ?</span>
                        </div>
                        <template #footer>
                            <Button label="No" icon="pi pi-times" @click="closeAddHall" class="p-button-text"/>
                            <Button label="Yes" icon="pi pi-check" @click="closeAddHall" class="p-button-text" autofocus />
                        </template>
                    </Dialog>
                    <DataTable :value="products2" editMode="row" dataKey="id" v-model:editingRows="editingRows" @row-edit-save="onRowEditSave" responsiveLayout="scroll">
                        <Column field="name" header="Name" style="width:50%">
                            <template #editor="{ data, field }">
                                <InputText v-model="data[field]" />
                            </template>
                        </Column>
                        <Column field="totalSeats" header="Total seats" style="width:50%">
                            <template #editor="{ data, field }">
                                <InputText v-model="data[field]" />
                            </template>
                        </Column>
                        <Column :rowEditor="true" style="width:10%; min-width:8rem" bodyStyle="text-align:center"></Column>
                    </DataTable>
                </div>
            </TabPanel>
            <TabPanel header="Seats">
                <Dropdown v-model="selectedCountry" :options="countries" optionLabel="name" :filter="true" placeholder="Select a cinema hall" :showClear="true">
                    <template #value="slotProps">
                        <div class="country-item country-item-value" v-if="slotProps.value">
                            <div>{{slotProps.value.name}}</div>
                        </div>
                        <span v-else>
                            {{slotProps.placeholder}}
                        </span>
                    </template>
                    <template #option="slotProps">
                        <div class="country-item">
                            <div>{{slotProps.option.name}}</div>
                        </div>
                    </template>
                </Dropdown>
            </TabPanel>
        </TabView>
    </div>
</template>

<script>
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import Dropdown from 'primevue/dropdown';
export default {
    name: 'CinemaView',
    components: {
        TabView,
        TabPanel,
        InputText,
        Button,
        DataTable,
        Column,
        Dialog,
        Dropdown
    },
    data() {
        return {
            displayConfirmation: false,
            displayAddHall: false,
            active: 3,
            items2: [
                {
                    label: 'Info',
                    icon: 'pi pi-fw pi-home'
                },
                {
                    label: 'Halls',
                    icon: 'pi pi-fw pi-building'
                },
                {
                    label: 'Seats',
                    icon: 'pi pi-fw pi-users'
                }
            ],
            countries: [
                {name: 'Australia', code: 'AU'},
                {name: 'Brazil', code: 'BR'},
                {name: 'China', code: 'CN'},
                {name: 'Egypt', code: 'EG'},
                {name: 'France', code: 'FR'},
                {name: 'Germany', code: 'DE'},
                {name: 'India', code: 'IN'},
                {name: 'Japan', code: 'JP'},
                {name: 'Spain', code: 'ES'},
                {name: 'United States', code: 'US'}
            ],
            selectedCountry: null,
        }
    },
    productService: null,
     created () {
    },
    methods: {
        openConfirmation() {
            this.displayConfirmation = true;
        },
        closeConfirmation() {
            this.displayConfirmation = false;
        },
        openAddHall() {
            this.displayAddHall = true;
        },
        closeAddHall() {
            this.displayAddHall = false;
        },
    }
}
</script>

<style scoped lang="scss">
::v-deep(.tabmenudemo-content) {
    padding: 2rem 1rem;
}
</style>
<template>
    <a :href="items[0].to + '/' + cinema.id"><Button class="p-button-raised p-button-rounded mr-3" disabled>{{items[0].label}}</Button></a>
    <a :href="items[1].to + cinema.id + '/halls'"><Button class="p-button-raised p-button-rounded mr-3" >{{items[1].label}}</Button></a>
    <card class="mt-3">
        <template #title>
            Update cinema
        </template>
        <template #content>
            <div class="ml-2">
                <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
                    <div class="card">
                        <h5>General information</h5>
                        <div class="field grid">
                            <label for="cinemaName" class="col-12 mb-2 md:col-2 md:mb-0">Name</label>
                            <div class="col-12 md:col-10">
                                <input id="cinemaName" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.name">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="imagePath" class="col-12 mb-2 md:col-2 md:mb-0">Image path</label>
                            <div class="col-12 md:col-10">
                                <input id="imagePath" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.imagePath">
                            </div>
                        </div>
                        <h5>Address information</h5>
                        <div class="field grid">
                            <label for="street" class="col-12 mb-2 md:col-2 md:mb-0">Street</label>
                            <div class="col-12 md:col-10">
                                <input id="street" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.street">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="city" class="col-12 mb-2 md:col-2 md:mb-0">City</label>
                            <div class="col-12 md:col-10">
                                <input id="city" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.city">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="zipCode" class="col-12 mb-2 md:col-2 md:mb-0">Zip Code</label>
                            <div class="col-12 md:col-10">
                                <input id="zipCode" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.zipCode">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="state" class="col-12 mb-2 md:col-2 md:mb-0">State</label>
                            <div class="col-12 md:col-10">
                                <input id="state" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.state">
                            </div>
                        </div>
                        <div class="field grid">
                            <label for="country" class="col-12 mb-2 md:col-2 md:mb-0">Country</label>
                            <div class="col-12 md:col-10">
                                <input id="country" type="text" class="text-base text-color surface-overlay p-2 border-1 border-solid surface-border border-round appearance-none outline-none focus:border-primary w-full" v-model="cinema.country">
                            </div>
                        </div>
                    </div>
                    <div class="text-right row mt-5">
                        <Button label="Submit" icon="pi pi-check" iconPos="left" class="p-button-success mr-2"  @click="submit()"/>
                        <Button label="Cancel" icon="pi pi-times" iconPos="left" class="p-button-info mr-2" @click="goBack()" />
                        <Button label="Delete" icon="pi pi-trash" @click="openConfirmation" class="p-button-danger" />
                        <Dialog header="Delete" v-model:visible="displayConfirmation" :style="{width: '350px'}" :modal="true">
                            <div class="confirmation-content">
                                <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
                                <span>Are you sure you want to delete ?</span>
                            </div>
                            <template #footer>
                                <Button label="No" icon="pi pi-times" @click="closeConfirmation" class="p-button-text"/>
                                <Button label="Yes" icon="pi pi-check" @click="deleteCinema(cinema.id)" class="p-button-text" autofocus />
                            </template>
                        </Dialog>
                    </div>
                </div>
        </template>
    </card>
</template>

<script>
import Message from 'primevue/message';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import CinemaService from '@/services/cinema-service';
import { useRoute } from 'vue-router';
import Card from 'primevue/card';
export default {
    name: 'CinemaView',
    components: {
        Button,
        Dialog,
        Message,
        Card
    },
    data() {
        return {
            displayConfirmation: false,
            displayAddHall: false,
            active: 1,
            cinema: {
                id: null,
                name: '',
                street: '',
                state: '',
                country: '',
                zipCode: '',
                imagePath: '',
                totalCinemaHalls: 1
            },
            items: [
                {
                    label: 'Cinema information',
                    icon: 'pi pi-fw pi-home',
                    to: '/cinemas',
                },
                {
                    label: 'Halls',
                    icon: 'pi pi-fw pi-calendar',
                    to: '/cinemas/'
                }
            ],
            showError: false,
            errorMessage: ''
        }
    },
    created () {
        const route = useRoute(); 
        const id = route.params.id;
        CinemaService.get(id).then((response) => {
            this.cinema = response.data
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })
    },
    methods: {
        openConfirmation() {
            this.displayConfirmation = true;
        },
        closeConfirmation() {
            this.displayConfirmation = false;
        },
        goBack() {
            this.$router.push({name: 'Cinemas'})
        },
        deleteCinema(id) {
            CinemaService.delete(id).then((response) => {
                    console.log(response.data)
                    this.$router.replace({name: 'Cinemas'})
                }).catch((error) => {
                    console.log(error.response.data)
            })
            this.displayConfirmation = false;
        },
        createCinema () {
            CinemaService.create(this.cinema).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Cinemas'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        updateCinema () {
            CinemaService.update(this.cinema.id,this.cinema).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Cinemas'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        submit () {
            if(this.cinema.id !== null){
                this.updateCinema();
            }else{
                console.log
                this.createCinema();
            }
        }
    }
}
</script>

<style scoped lang="scss">
a {
    text-decoration: none;
    color: black;
}
.field * {
    display: block;
}
</style>
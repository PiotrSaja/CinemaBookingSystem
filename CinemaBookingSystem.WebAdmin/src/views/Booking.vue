<template>
    <div class="card">
        <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
        <h2>Please insert data</h2>
            <h3>General information:</h3>
            <div class="row">
                 <div class="field">
                        <label class="mr-2" for="cinemaName">Cinema name</label>
                        <InputText id="cinemaName" type="text" aria-describedby="username2-help" v-model="cinema.name"/>
                        <small id="username2-help" class="p-error" v-if="false">Username is not available.</small>
                </div>
                <div class="field">
                        <label class="mr-2" for="imagePath">Image path</label>
                        <InputText id="imagePath" type="url" aria-describedby="username2-help" v-model="cinema.imagePath"/>
                        <small id="username2-help" class="p-error" v-if="false">Username is not available.</small>
                </div>
            </div>
            <h3 class="mt-5">Address:</h3>
            <div class="row">
                <div class="field">
                        <label class="mr-2" for="street">Street</label>
                        <InputText id="street" type="text" aria-describedby="username2-help" v-model="cinema.street"/>
                </div>
                 <div class="field">
                        <label class="mr-2" for="city">City</label>
                        <InputText id="city" type="text" aria-describedby="username2-help" v-model="cinema.city"/>
                </div>
                 <div class="field">
                        <label class="mr-2" for="zipCode">Zip Code</label>
                        <InputText id="zipCode" type="text" aria-describedby="username2-help" v-model="cinema.zipCode"/>
                </div>
                 <div class="field">
                        <label class="mr-2" for="state">State</label>
                        <InputText id="state" type="text" aria-describedby="username2-help" v-model="cinema.state"/>
                </div>
                 <div class="field">
                        <label class="mr-2" for="country">Country</label>
                        <InputText id="country" type="text" aria-describedby="username2-help" v-model="cinema.country"/>
                </div>
            </div>
            <div class="row ml-2 mt-3">
                <Button label="Submit" icon="pi pi-check" iconPos="right" class="p-button-success mr-2"  @click="submit()"/>
                <Button label="Cancel" icon="pi pi-times" iconPos="right" class="p-button-info mr-2" @click="goBack()" />
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

<script>
import Message from 'primevue/message';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import CinemaService from '@/services/cinema-service';
import { useRoute } from 'vue-router'
export default {
    name: 'BookingView',
    components: {
        Button,
        Dialog,
        InputText,
        Message
    },
    data() {
        return {
            displayConfirmation: false,
            displayAddHall: false,
            active: 1,
            cinemaHall: 1,
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
                    label: 'Cinema informations',
                    icon: 'pi pi-fw pi-home',
                    to: '/cinema',
                },
                {
                    label: 'Cinema Halls',
                    icon: 'pi pi-fw pi-calendar',
                    to: '/cinema/'
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
        createMovie () {
            CinemaService.create(this.cinema).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Cinemas'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        updateMovie () {
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
                this.updateMovie();
            }else{
                console.log
                this.createMovie();
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
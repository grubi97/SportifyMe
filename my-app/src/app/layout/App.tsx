import React, { Fragment, useContext, useEffect } from "react";
import { Container } from "semantic-ui-react";
import NavBar from "../../features/nav/NavBar";
import { observer } from "mobx-react-lite";
import ActivityDashbord from "../../features/activities/dashbord/ActivityDashbord";
import {
  Route,
  withRouter,
  RouteComponentProps,
  Switch
} from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import ActivityForm from "../../features/activities/form/ActivityForm";
import AcitivityDetails from "../../features/activities/details/AcitivityDetails";
import NotFound from "./Notfound";
import {ToastContainer} from 'react-toastify'
import { LoginForm } from "../../features/user/LoginForm";
import { RootStoreContext } from "../stores/rootStore";
import { LoadingComponent } from "./LoadingComponent";
import  ModalContainer  from "../common/modals/ModalContainer";
import ProfilePage from "../../features/profiles/ProfilePage";

const App: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore=useContext(RootStoreContext);
  const{setAppLoaded,token,appLoaded}=rootStore.commonStore;
  const{getUser}=rootStore.userStore;


  useEffect(()=>{
    if(token){
      getUser().finally(()=>setAppLoaded())
    }else{
      setAppLoaded();
    }
  },[getUser,setAppLoaded,token])



  if(!appLoaded ) return <LoadingComponent content='Loading app'/>
  return (
    <Fragment>
      <ModalContainer/>
      <ToastContainer position='bottom-right'/>
      <Route exact path="/" component={HomePage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <Fragment>
            <NavBar />
            <Container style={{ marginTop: "7em" }}>
              <Switch>
                <Route exact path="/activities" component={ActivityDashbord} />
                <Route path="/activities/:id" component={AcitivityDetails} />
                <Route
                  key={location.key}
                  path={["/createActivity", "/manage/:id"]}
                  component={ActivityForm}
                />

                <Route path='/profile/:username' component={ProfilePage} />
                <Route component={NotFound} />
              </Switch>
            </Container>
          </Fragment>
        )}
      />
    </Fragment>
  );
};
export default withRouter(observer(App)); //observer je za activity store, s wthrouter ima pritup location propertijjima
//'/(.+)' sve razlicito od toga nije home page

//svo koji rade s observable su observeri!!!

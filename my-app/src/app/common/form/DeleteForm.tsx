import React, { useContext } from "react";
import { Header, Button } from "semantic-ui-react";
import { RootStoreContext } from "../../stores/rootStore";
import { IActivity } from "../../models/activity";


export const DeleteForm: React.FC<{ activity: IActivity }> = ({ activity }) => {
  const rootStore = useContext(RootStoreContext);

  const { closeModal } = rootStore.modalStore;
  const { deleteActivity } = rootStore.activityStore;

  return (
    <div>
      <Header>Are You sure you want to delete this activity?</Header>
      <Button
        color="red"
        content="Delete"
        onClick={(e) => deleteActivity(e,activity.id).then(()=>closeModal())}
      ></Button>
      <Button
        type="button"
        content="Cancel"
        onClick={() => closeModal()}
      ></Button>
    </div>
  );
};

import React, { useRef } from "react";
import Cropper from "react-cropper";
import "cropperjs/dist/cropper.css";
import { type } from "os";

interface IProps {
  setImage: (file: Blob) => void;
  imagePreview: string;
}

const PhotoWidgetCropper: React.FC<IProps> = ({ setImage, imagePreview }) => {
  const cropper = useRef<Cropper>(null);
  const cropImage = () => {
    if (
      cropper.current &&
      typeof cropper.current.getCroppedCanvas() === "undefined"
    ) {
      return;
    }
    cropper &&
      cropper.current &&
      cropper.current.getCroppedCanvas().toBlob((blob: any) => {
        setImage(blob);
      }, "image/jpeg");
  };
  return (
    <Cropper
      ref={cropper}//izmjena u d.ts.filu
      src={imagePreview}
      style={{ height: 200, width: "100%" }}
      // Cropper.js options
      aspectRatio={1 / 1} //square image
      guides={false}
      viewMode={1} //ne moze slik izvan canvasa
      dragMode="move"
      scalable={true}
      cropBoxMovable={true}
      cropBoxResizable={true}
      crop={cropImage}
      preview='.img-preview'
    />
  );
};

export default PhotoWidgetCropper;

if NOT EXISTS(select CAID from classassociation where ParentClass='MeasurementItem' and ChildClass='MeasurementItem' and AssociationTypeID=3)
BEGIN
	INSERT INTO CLASSASSOCIATION(ParentClass,ChildClass,AssociationTypeID,Caption,CopyIncluded,IsDefault)
	values ('MeasurementItem','MeasurementItem',3,'MeasurementItem Decomposition',0,0)
	
END